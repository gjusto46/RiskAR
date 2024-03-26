using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Linq;
using Firebase.Database;
using ScripableObjects;
using UI;
using Unity.Mathematics;
using UnityEngine.Events;

namespace Image_Tracking
{
    public class ImageTrackedSelection : MonoBehaviour
    {
        [SerializeField] private ObjectsPositionInfo _objectsPositionInfo;
        [SerializeField] private ConnectionData _connectionData;
        [SerializeField] private ARTrackedImageManager _arTrackedImageManager;
        [SerializeField] private ImageLibraryData _imageLibraryData;
        // public Dictionary<Guid, GameObject> _instantiated = new Dictionary<Guid, GameObject>();
        public Dictionary<string, GameObject> _instantiated = new Dictionary<string, GameObject>();
        [SerializeField] private ARAnchorManager _arAnchorManager;
        [SerializeField] private TYPE_ANCHOR _typeAnchor; 
        [SerializeField] private GameObject _prefab;

        [SerializeField] private UnityEvent _onImageFound;
        [SerializeField] private UnityEvent _onImageRecognized;
        [SerializeField] private UnityEvent _onImageDontRecognized;
        
        [SerializeField] private UnityEvent _onSavingData;
        [SerializeField] private UnityEvent _onSavedData;
        

        // private string _referenceGUIDInsantiated;
        private ARPlane _planeDetected;
        private Pose _pose;
        private ARRaycastHit _hit;
        public ARPlaneManager planeManager;
        private bool _imageFound;
        private bool _dataFound;
        Transform _imageTransform;
        // Dictionary<Guid, Transform> _imageTransform = new Dictionary<Guid, Transform>();

        enum TYPE_ANCHOR
        {
            NONE = 0,
            POINT = 1,
            PLANE = 2
        }

        public bool ImageFound
        {
            get => _imageFound;
            set
            {
                _imageFound = value;
                if (_imageFound)
                {
                    _onImageFound?.Invoke();
                }
            }
        }
        private void Awake()
        {
            _arTrackedImageManager = GetComponent<ARTrackedImageManager>();
            for (int i = 0; i < _objectsPositionInfo._listPositions._PositionInfos.Count; i++)
            {
                Debug.Log(_objectsPositionInfo._listPositions._PositionInfos[i].idObject);
            }
        }

        private void OnEnable()
        {
            // _arTrackedImageManager.trackedImagesChanged += ArTrackedImageManagerOntrackedImagesChanged;
            _arTrackedImageManager.trackedImagesChanged += ArTrackedImageManagerOntrackedImagesChangedReference;
        }

        private void OnDisable()
        {
            _arTrackedImageManager.trackedImagesChanged -= ArTrackedImageManagerOntrackedImagesChangedReference;
        }

        private void ArTrackedImageManagerOntrackedImagesChangedReference(ARTrackedImagesChangedEventArgs obj)
        {
            for (int i = 0; i < obj.added.Count; i++)
            {
                if (_imageLibraryData.prefabsDictionary.TryGetValue(obj.added[i].referenceImage.guid, out var prefab))
                {
                    GeneratePoins(obj.added[i]);
                    ImageFound = true;
                }
            }
        }

        public void GeneratePoins(ARTrackedImage image)
        {
            _imageTransform = image.transform;
            if (!_objectsPositionInfo.isSavedInfo)
            {
                _dataFound = false;
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

            _dataFound = true;

        }

        public void ShowStateImageRecognition()
        {
            if (_dataFound)
            {
                _onImageRecognized?.Invoke();

            }
            else
            {
                _onImageDontRecognized?.Invoke();

            }
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
                objectTemp.transform.parent = null;
                objectTemp.transform.localScale = posInfoTemp.globalScale;
                // objectTemp.transform.rotation = posInfoTemp.globalRotation;
                pair.Value.gameObject.AddComponent<ARAnchor>();
                targetO.UpdateRigState(true);

                yield return new WaitForEndOfFrame();
            }
        }
        public void GenerateDirectModels(ARTrackedImage image)
        {
            Debug.Log(image.name);
            _imageTransform = image.transform;
            for (int j = 0; j < _objectsPositionInfo._listPositions._PositionInfos.Count; j++)
            {
                Debug.Log(_objectsPositionInfo._listPositions._PositionInfos[j].idObject);
                // actual position info
                var positionInfo = _objectsPositionInfo._listPositions._PositionInfos[j];
                // actual model info 
                var modelInfo = _objectsPositionInfo._ModelsToInstanciates.FirstOrDefault(
                    x => x.idObject == positionInfo.idObject
                );

                Vector3 positionTemp = positionInfo.position;
                positionTemp = _imageTransform.TransformPoint(positionTemp);
                var rotationTemp = _imageTransform.rotation * positionInfo.rotation;
                var reference = Instantiate(modelInfo.prefab,positionTemp, rotationTemp);
                var test = Instantiate(_prefab, _imageTransform.transform);
                test.transform.localPosition = positionInfo.position;
                reference.transform.localScale = positionInfo.scale; 

                reference.GetComponent<CalculatePositionAR>().IsUbicated = true;
                _instantiated.Add(_objectsPositionInfo._listPositions._PositionInfos[j].idObject, reference);
            }
        }
        public void AddObjectInstantiated(string id, GameObject gameObjectInstantiated)
        {
            _instantiated.Add(id, gameObjectInstantiated);
        }
        
        
        // private void ArTrackedImageManagerOntrackedImagesChanged(ARTrackedImagesChangedEventArgs obj)
        // {
        //     for (int i = 0; i < obj.added.Count; i++)
        //     {
        //         
        //         var minLocalScalar = Mathf.Min(obj.added[i].size.x, obj.added[i].size.y) / 2;
        //         obj.added[i].transform.localScale = new Vector3(minLocalScalar, minLocalScalar, minLocalScalar);
        //         AssignPrefab(obj.added[i]);
        //     }
        // }
        public void SetPrefabForReferenceImage(XRReferenceImage referenceImage, GameObject alternativePrefab)
        {
            _imageLibraryData.prefabsDictionary[referenceImage.guid] = alternativePrefab;
            if (_instantiated.TryGetValue(referenceImage.guid.ToString(), out var instantiatedPrefab))
            {
                _instantiated[referenceImage.guid.ToString()] = Instantiate(alternativePrefab, instantiatedPrefab.transform.parent);
                Destroy(instantiatedPrefab);
            }
        }
        // void AssignPrefab(ARTrackedImage trackedImage)
        // {
        //
        //     if (_imageLibraryData.prefabsDictionary.TryGetValue(trackedImage.referenceImage.guid, out var prefab))
        //     {
        //         var positionInfo = FindObjectPosition(trackedImage.referenceImage.guid);
        //         _imageTransform.Add(trackedImage.referenceImage.guid, trackedImage.transform);
        //         if (positionInfo != null)
        //         {
        //             _instantiated[trackedImage.referenceImage.guid] = Instantiate(prefab, trackedImage.transform);
        //             _instantiated[trackedImage.referenceImage.guid].GetComponent<CalculatePositionAR>().IsUbicated = true;
        //             _instantiated[trackedImage.referenceImage.guid].transform.localPosition = new Vector3(
        //                 (float) positionInfo.positionX, (float) positionInfo.positionY, (float) positionInfo.positionZ);
        //             _instantiated[trackedImage.referenceImage.guid].transform.localRotation = positionInfo.rotation;
        //             TargetO targetO = _instantiated[trackedImage.referenceImage.guid].GetComponent<TargetO>();
        //             targetO.UpdateRigState(false);
        //             _instantiated[trackedImage.referenceImage.guid].transform.localScale = trackedImage.transform.InverseTransformVector(positionInfo.scale);
        //             targetO.UpdateRigState(true);
        //         }
        //         else
        //         {
        //             _instantiated[trackedImage.referenceImage.guid] = Instantiate(prefab, trackedImage.transform);
        //         }
        //         // _instantiated[trackedImage.referenceImage.guid].AddComponent<ARAnchor>();
        //         
        //         // ARAnchor arAnchor = new ARAnchor();
        //         _referenceGUIDInsantiated = trackedImage.referenceImage.guid;
        //         
        //         
        //     }
        // }
        public void DeleteTrackablePlanes()
        {
            var trackable = gameObject.transform.Find("Trackables").gameObject;

            for (int i = 0; i < trackable.transform.childCount; i++)
            {
                var visualizer = trackable.transform.GetChild(i).GetComponent<ARPlaneMeshVisualizer>();
                if ( visualizer)
                {
                    visualizer.enabled = false;
                }
            }
            // _instantiated[_referenceGUIDInsantiated].transform.parent = null;
            // _instantiated[_referenceGUIDInsantiated].transform.AddComponent<ARAnchor>();
            // _instantiated[_referenceGUIDInsantiated].gameObject.AddComponent<ARAnchor>();
            // CreateAnchor(_hit);
            // switch (_typeAnchor)
            // {
            //     case TYPE_ANCHOR.NONE:
            //         break;
            //     case TYPE_ANCHOR.PLANE:
            //         var planeManager = GetComponent<ARPlaneManager>();
            //         ARAnchor anc = _instantiated[_referenceGUIDInsantiated].transform.AddComponent<ARAnchor>();
            //         anc = _arAnchorManager.AttachAnchor(_planeDetected, _pose);
            //         Debug.Log("plane");
            //         break;
            //     case TYPE_ANCHOR.POINT:
            //         _instantiated[_referenceGUIDInsantiated].transform.AddComponent<ARAnchor>();
            //         Debug.Log("point");
            //         break;
            // }


        }

        ARAnchor CreateAnchor(in ARRaycastHit hit)
        {
            ARAnchor anchor = null;

            // If we hit a plane, try to "attach" the anchor to the plane
            if (hit.trackable is ARPlane plane)
            {
                planeManager = GetComponent<ARPlaneManager>();
                if (planeManager != null)
                {

                    if (_prefab != null)
                    {
                        var oldPrefab = _arAnchorManager.anchorPrefab;
                        _arAnchorManager.anchorPrefab = _prefab;
                        anchor = _arAnchorManager.AttachAnchor(plane, hit.pose);
                        _arAnchorManager.anchorPrefab = oldPrefab;
                    }
                    else
                    {
                        anchor = _arAnchorManager.AttachAnchor(plane, hit.pose);
                    }

                    return anchor;
                }
            }

            if (_prefab != null)
            {
                // Note: the anchor can be anywhere in the scene hierarchy
                var gameObject = Instantiate(_prefab, hit.pose.position, hit.pose.rotation);

                // Make sure the new GameObject has an ARAnchor component
                anchor = ComponentUtils.GetOrAddIf<ARAnchor>(gameObject, true);
            }
            else
            {
                var gameObject = new GameObject("Anchor");
                gameObject.transform.SetPositionAndRotation(hit.pose.position, hit.pose.rotation);
                anchor = gameObject.AddComponent<ARAnchor>();
            }


            return anchor;
        }

        public void GetPlaneDetected(ARPlane arPlane,Pose pose, ARRaycastHit hit)
        {
            _planeDetected = arPlane;
            _pose = pose;
            _hit = hit;
        }
        public void ChoiceTypeAnchor(string valueType)
        {
            if (Enum.TryParse( valueType, out TYPE_ANCHOR result))
            {
                _typeAnchor = result; 

                // switch (result)
                // {
                //     case TYPE_ANCHOR.NONE:
                //         Debug.Log("none");
                //         break;
                //     case TYPE_ANCHOR.PLANE:
                //         Debug.Log("plane");
                //         break;
                //     case TYPE_ANCHOR.POINT:
                //         Debug.Log("point");
                //         break;
                // }
            }
        } 
        public void ChoiceTypeAnchor(int index)
        {
            _typeAnchor = (TYPE_ANCHOR) index; 
            // switch ((TYPE_ANCHOR)index)
            // {
            //     case TYPE_ANCHOR.NONE:
            //         Debug.Log("none");
            //         break;
            //     case TYPE_ANCHOR.PLANE:
            //         Debug.Log("plane");
            //         break;
            //     case TYPE_ANCHOR.POINT:
            //         Debug.Log("point");
            //         break; 
            // }
        }

        public PositionInfo FindObjectPosition(Guid idObjectToFind)
        {
            PositionInfo positionInfo = _objectsPositionInfo._listPositions._PositionInfos.FirstOrDefault
                    (
                        info => info.idObject == idObjectToFind.ToString()
                    );
            if (positionInfo != null)
            {
                return positionInfo;
            }
            return null;
            
        }

        public void GeneratePositionInfo()
        {
            _onSavingData?.Invoke();
            if (_objectsPositionInfo.isSavedInfo)
            {
                _objectsPositionInfo._listPositions._PositionInfos.Clear();
                SaveInfo();
            }
            else
            {
                SaveInfo();
            }

            _objectsPositionInfo.isSavedInfo = true;
        }

        private void SaveInfo()
        {
            foreach (KeyValuePair<String,GameObject> valuePair in _instantiated)
            {
                var position = _imageTransform.InverseTransformPoint(valuePair.Value.transform.position);
                var transformInstant = valuePair.Value.transform;
                PositionInfo newPositionInfo = new PositionInfo
                {
                    idObject = valuePair.Key,
                    position = position,
                    rotation = Quaternion.Inverse(_imageTransform.rotation) * transformInstant.rotation,
                    scale = _imageTransform.TransformVector(transformInstant.lossyScale),
                    globalPosition = position,
                    globalScale = transformInstant.lossyScale,
                    globalRotation = transformInstant.rotation
                };
                _objectsPositionInfo._listPositions._PositionInfos.Add(newPositionInfo);
            }
            // var stringJson = JsonUtility.ToJson(_objectsPositionInfo._listPositions._PositionInfos);
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            var stringJson = JsonUtility.ToJson(_objectsPositionInfo._listPositions);
#if UNITY_EDITOR
            var data = reference.Child(_connectionData.referenceLocal);
#else
        var data = reference.Child(_connectionData.referenceProduction);
#endif
            // Debug.Log(data.GetValueAsync());
            data.SetValueAsync(stringJson);
            // PlayerPrefs.SetString("Positions", stringJson);
            // PlayerPrefs.Save();
            _onSavedData?.Invoke();
        }
        public void DeleteInfo()
        {
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
#if UNITY_EDITOR
            var data = reference.Child(_connectionData.referenceLocal);
#else
        var data = reference.Child(_connectionData.referenceProduction);
#endif
            // Debug.Log(data.GetValueAsync());
            data.RemoveValueAsync();
            // PlayerPrefs.DeleteAll();
            // PlayerPrefs.Save();
        }
    }
}