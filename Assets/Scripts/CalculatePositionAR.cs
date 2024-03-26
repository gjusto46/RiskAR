using System;
using System.Collections;
using System.Collections.Generic;
using Image_Tracking;
using UI;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class CalculatePositionAR : MonoBehaviour
{
    Vector3 _rayDirection = Vector3.down;
    private List<ARRaycastHit> _hits = new List<ARRaycastHit>();
    private bool _isUbicated;
    [SerializeField] private GameObject _floor;
    public PositionInfo _positionInfo;
    private ImageTrackedSelection imageTrackedSelection;
    private ARRaycastManager _arRaycastManager;
    private Ray _ray;
    private Vector3 _centerPosition;
    private Vector3 _centerLocalPosition;
    public bool IsUbicated
    {
        get => _isUbicated;
        set => _isUbicated = value;
    }
    private void Start()
    {
        imageTrackedSelection = FindObjectOfType<ImageTrackedSelection>();
        _arRaycastManager = FindObjectOfType<ARRaycastManager>();
        _centerPosition = GetComponent<TargetO>().TransformReference.position;
        _centerLocalPosition = transform.InverseTransformPoint(_centerPosition);
    }

    private void Update()
    {
        if (_isUbicated)
        {
            if (_floor)
            {
                _floor.SetActive(true);
            }
            return;
        }

        _ray = new Ray(_centerPosition, _rayDirection);
        if (_arRaycastManager.Raycast(_ray, _hits, TrackableType.PlaneWithinPolygon))
        {
            foreach (ARRaycastHit raycastHit in _hits)
            {
                Debug.Log(raycastHit.trackable.name);
            }
            if (_hits[0].trackable is ARPlane arPlane)
            {
                
                imageTrackedSelection.GetPlaneDetected(arPlane, _hits[0].pose, _hits[0]);
                var obj =GameObject.CreatePrimitive(PrimitiveType.Cube);
                obj.transform.position = _hits[0].pose.position;
                obj.transform.localScale = Vector3.one * .1f;
                // var obj1 =GameObject.CreatePrimitive(PrimitiveType.Sphere);
                // obj1.transform.position = transform.position;
                _isUbicated = true;
                // var posTemp = transform.position;
                // posTemp.y = _hits[0].pose.position.y;
                transform.position = _hits[0].pose.position;
                transform.rotation = _hits[0].pose.rotation;

                Debug.Log($"anchor:{transform.TransformPoint(-_centerLocalPosition)}");
                transform.position = transform.TransformPoint(Vector3.zero);
                // transform.position = transform.InverseTransformPoint(_centerPosition);
                // transform.position = posTemp;
            }
        }
    }
}
