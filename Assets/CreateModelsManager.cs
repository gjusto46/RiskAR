using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Image_Tracking;
using TMPro;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class CreateModelsManager : PressInputBase
{
    [SerializeField] private TMP_Dropdown _dropdown;
    [SerializeField] private ObjectsPositionInfo _objectsPositionInfo;
    [FormerlySerializedAs("_imageTrackedManagerSelection")] [SerializeField] private ImageTrackedSelection imageTrackedSelection;
    [SerializeField] private ARRaycastManager _arRaycastManager;
    [SerializeField] private Button _buttonAccept;
    [SerializeField] private Button _buttonCancel;
    [SerializeField] private LayerMask _layerMaskVertical;
    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    bool _pressed;
    protected override void OnPressCancel() => _pressed = false;
    protected override void OnPress(Vector3 position) => _pressed = true;
    public GameObject spawnedObject { get; private set; }
    private TargetO _targetO;
    private GameObject _placedPrefab;
    private string _index;
    private int _idObject;
    private bool _haveModel;
    private bool _isCreated;
    private bool _reubicate;
    [Serializable]
    public struct Models
    {
        public int id;
        public GameObject prefab;
    }
    protected override void Awake()
    {
        base.Awake();
        // _arRaycastManager = GetComponent<ARRaycastManager>();
    }

    private void Start()
    {
        _dropdown.onValueChanged.AddListener(ModelToInstance); 
    }

    private void ModelToInstance(int arg0)
    {
        if (arg0 == 0)
        {
            return;
        }
        _index = arg0.ToString();
        // _placedPrefab = _objectsPositionInfo._ModelsToInstanciates.FirstOrDefault(x => x.prefab.name == arg0.ToString())?.prefab;
        var info = _objectsPositionInfo._ModelsToInstanciates.FirstOrDefault(x =>
            x.prefab.name == _dropdown.options[arg0].text);
        _placedPrefab = info?.prefab;
        _idObject = Int32.Parse(info.idObject);
        _haveModel = true;
        _buttonAccept.gameObject.SetActive(true);
        _buttonAccept.interactable = false;
        _buttonCancel.gameObject.SetActive(true);
    }

    public void Reubicate(bool valueBool)
    {
        _reubicate = valueBool;
        if (!_reubicate)
        {
            _haveModel = false;
            spawnedObject = null;
            _targetO = null;
        }
    }
    public void ReubicateModel(UpdateTransformManager _transformManager)
    {
        if (_reubicate)
        {
            _haveModel = true;
            spawnedObject = _transformManager.TargetO.gameObject;
            _targetO = spawnedObject.GetComponent<TargetO>();
        }
        
    }

    public void CancelReubicate()
    {
        if (_reubicate)
        {
            _haveModel = false;
            spawnedObject = null;
            _targetO = null;
            Reubicate(false);
        }
    }
    public void DisableModel()
    {
        _buttonAccept.gameObject.SetActive(false);
        _buttonCancel.gameObject.SetActive(false); 

        _haveModel = false;
        if (!spawnedObject.GetComponent<ARAnchor>())
        {
            spawnedObject.AddComponent<ARAnchor>();
        }
        _placedPrefab = null;
        _targetO.HideRenderMarker();
        spawnedObject = null;
        _targetO = null;
    }

    public void CancelModel()
    {
        _haveModel = false;
        _placedPrefab = null;
        _buttonAccept.gameObject.SetActive(false);
        _buttonCancel.gameObject.SetActive(false);

    }

    private void LateUpdate()
    {
        if (Pointer.current == null || _pressed == false || EventSystem.current.currentSelectedGameObject ||
            !_haveModel)
        {
            return;
        }
            

        var touchPosition = Pointer.current.position.ReadValue();

        
        if (_targetO && _targetO.IsVertical)
        {
            Ray ray = Camera.main.ScreenPointToRay(Pointer.current.position.ReadValue());
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, _layerMaskVertical))
            {
                Debug.Log(hit.transform.gameObject.layer);
                spawnedObject.transform.position = hit.point;
                spawnedObject.transform.up = hit.normal;
                var rotationEuler = spawnedObject.transform.eulerAngles;
                spawnedObject.transform.eulerAngles = rotationEuler;
                // spawnedObject.transform.Rotate(spawnedObject.transform.up, 90);
                Debug.DrawRay(hit.point, hit.normal, Color.green);
            }
        }
        else
        {
            if (_arRaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                // Raycast hits are sorted by distance, so the first one
                // will be the closest hit.
                var hitPose = s_Hits[0].pose;

                if (!spawnedObject)
                {
                    spawnedObject = Instantiate(_placedPrefab, hitPose.position, hitPose.rotation);
                    imageTrackedSelection.AddObjectInstantiated(_idObject.ToString(), spawnedObject);
                    spawnedObject.GetComponent<CalculatePositionAR>().IsUbicated = true;
                    _targetO = spawnedObject.GetComponent<TargetO>(); 
                    _targetO.ShowRenderMarker();
                    _buttonAccept.interactable = true;
                    _buttonCancel.gameObject.SetActive(false);
                    _objectsPositionInfo._ModelsToInstanciates.FirstOrDefault(x => x.idObject == _idObject.ToString())!.instantiated = true;
                }
                else
                {
                    spawnedObject.transform.position = hitPose.position;
                    spawnedObject.transform.rotation = hitPose.rotation;
                }
            }
        }
    }
}
