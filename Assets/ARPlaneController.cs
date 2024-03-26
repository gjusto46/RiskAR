using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARPlaneController : MonoBehaviour
{
    [SerializeField] private ARPlaneManager _arPlaneManager;
    private Transform _trackableContent;

    private void Start()
    {
        _trackableContent = transform.Find("Trackables");
        
    }

    public void DeletePlanes()
    {
        bool isDetectingPlane = _arPlaneManager.enabled;
        StopPlaneDetection();
        foreach (var arPlane in _arPlaneManager.trackables)
        {
            if (!arPlane)
            {
                Debug.Log("eliminado");
            }
            else
            {
                Destroy(arPlane.gameObject);
            }
        }

        if (isDetectingPlane)
        {
            RestorePlaneDetection();
        }
    }

    public void StopPlaneDetection()
    {
        _arPlaneManager.enabled = false;
    }

    public void RestorePlaneDetection()
    {
        _arPlaneManager.enabled = true;
    }
}
