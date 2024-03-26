using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;

public class ImageTrackedManagerVisualization : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager _arTrackedImageManager;
    Transform _imageTransform;
    [SerializeField] private ImageLibraryData _imageLibraryData;
    [SerializeField] private ObjectsPositionInfo _objectsPositionInfo;
    public Dictionary<string, GameObject> _instantiated = new Dictionary<string, GameObject>();
    [SerializeField] private UnityEvent _onImageRecognized;
    [SerializeField] private UnityEvent _onImageDontRecognized;
    [SerializeField] private UnityEvent _onInit;
    [SerializeField] private UnityEvent _onImageFound;
    
    

    private void Awake()
    {
        _arTrackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    private void Start()
    {
        _onInit?.Invoke();
    }

    private void OnEnable()
    {
        _arTrackedImageManager.trackedImagesChanged += ArTrackedImageManagerOntrackedImagesChanged;
    }

    private void OnDisable()
    {
        _arTrackedImageManager.trackedImagesChanged -= ArTrackedImageManagerOntrackedImagesChanged;
    }

    private void ArTrackedImageManagerOntrackedImagesChanged(ARTrackedImagesChangedEventArgs obj)
    {
        for (int i = 0; i < obj.added.Count; i++)
        {
            if (_imageLibraryData.prefabsDictionary.TryGetValue(obj.added[i].referenceImage.guid, out var prefab))
            {
                AssignPrefab(obj.added[i]);
            }
        }
    }
    void AssignPrefab(ARTrackedImage image)
    {
        _onImageFound?.Invoke();
        _imageTransform = image.transform;
        if (!_objectsPositionInfo.isSavedInfo)
        {
            _onImageDontRecognized?.Invoke();
            return;
        }
        for (int j = 0; j < _objectsPositionInfo._listPositions._PositionInfos.Count; j++)
        {
            // actual position info
            var positionInfo = _objectsPositionInfo._listPositions._PositionInfos[j];
            // actual model info 
            var modelInfo = _objectsPositionInfo._ModelsToInstanciates.FirstOrDefault(
                x => x.idObject == positionInfo.idObject
            );
            if (modelInfo != null)
            {
                var reference = Instantiate(modelInfo.prefab, _imageTransform.transform);
                reference.transform.localPosition = positionInfo.position;
                reference.transform.localScale = _imageTransform.transform.InverseTransformVector(positionInfo.scale);
                reference.transform.localRotation = positionInfo.rotation;
                reference.GetComponent<CalculatePositionAR>().IsUbicated = true;
                reference.GetComponent<CalculatePositionAR>()._positionInfo = positionInfo;
                _instantiated.Add(_objectsPositionInfo._listPositions._PositionInfos[j].idObject, reference);
            }
        }
        _onImageRecognized?.Invoke();
    }
    public void GenerateGlobalPosition()
    {
        StartCoroutine(IGenerateGlobalPosition());
    }
    IEnumerator IGenerateGlobalPosition()
    {
        yield return null;
        foreach (KeyValuePair<string,GameObject> pair in _instantiated)
        {
            TargetO targetO = pair.Value.GetComponent<TargetO>();
            targetO.UpdateRigState(false);
            var objectTemp = pair.Value;
            var posInfoTemp = objectTemp.GetComponent<CalculatePositionAR>()._positionInfo;
            objectTemp.transform.localPosition = posInfoTemp.position;
            pair.Value.transform.parent = null;
            objectTemp.transform.localScale = posInfoTemp.globalScale;
            // objectTemp.transform.rotation = posInfoTemp.globalRotation;
            pair.Value.gameObject.AddComponent<ARAnchor>();
            targetO.UpdateRigState(true);
            yield return new WaitForEndOfFrame();
        }
    }

}
