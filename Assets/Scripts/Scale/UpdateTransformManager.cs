using System;
using System.Collections;
using System.Collections.Generic;
using Image_Tracking;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class UpdateTransformManager : MonoBehaviour
{
    [SerializeField] private GameObject _UIObject;
    [FormerlySerializedAs("_imageTrackedManagerSelection")] [SerializeField] private ImageTrackedSelection imageTrackedSelection;

    [FormerlySerializedAs("_imageTrackedManagerSelection")] [SerializeField]
    private ImageTrackedManagerVisualization imageTrackedVisualization;
    [SerializeField] private float _scaleFactorDrag = 50;
    [SerializeField] private TextMeshProUGUI _objectName;
    [SerializeField] private TransitionAnimation _transitionAnimation;
    [SerializeField] private ToggleGroup _toggleGroup;
    [FormerlySerializedAs("_scaleObject")] [SerializeField] private TargetO targetO;

    public TargetO TargetO
    {
        get => targetO;
        set => targetO = value;
    }

    public void AddObject(TargetO targetO)
    {
        _toggleGroup.SetAllTogglesOff();
        if (this.targetO)
        {
            this.targetO.enabled = false;
        }
        this.targetO = targetO;
        var transform1 = this.targetO.transform;
        this.targetO.InitialPosition = transform1.position;
        this.targetO.InitialQuaternion = transform1.rotation;
        this.targetO.InitialScale = transform1.localScale;
        _objectName.text = this.targetO.name;
        ShowTarget();
        // _UIObject.SetActive(true);
    }

    public void CancelActualPosition()
    {
        var transformTemp = targetO.transform;
        transformTemp.position = targetO.InitialPosition;
        transformTemp.rotation = targetO.InitialQuaternion;
        transformTemp.localScale = targetO.InitialScale;
    }
    public void RemoveObjects()
    {
        _toggleGroup.SetAllTogglesOff();
        DeleteTarget();
        // _UIObject.SetActive(false);
        _objectName.text = "";
        if (targetO)
        {
            targetO.HideRenderMarker();
            targetO.enabled = false;
            targetO = null;
        }
        
    }

    public void UpdateXMove(float moveValue)
    {
        targetO.transform.position += targetO.TransformReference.transform.right * moveValue * Time.deltaTime / _scaleFactorDrag;
    }
    public void UpdateYMove(float moveValue)
    {
        targetO.transform.position += targetO.TransformReference.transform.up * moveValue * Time.deltaTime / _scaleFactorDrag;

    }
    public void UpdateZMove(float moveValue)
    {
        targetO.transform.position += targetO.TransformReference.transform.forward * moveValue * Time.deltaTime / _scaleFactorDrag;
    }
    public void RotateXAxis(float valueRotation)
    {
        targetO.transform.RotateAround(targetO.TransformReference.position, targetO.TransformReference.right, valueRotation * Time.deltaTime);
    }
    public void RotateYAxis(float valueRotation)
    {
        targetO.transform.RotateAround(targetO.TransformReference.position, targetO.TransformReference.up, valueRotation * Time.deltaTime);
    }
    public void RotateZAxis(float valueRotation)
    {
        targetO.transform.RotateAround(targetO.TransformReference.position, targetO.TransformReference.forward, valueRotation * Time.deltaTime);
    }

    public void DisableARAnchor(bool toggleOn)
    {
        if (!toggleOn)
        {
            return;
        }

        var arAnchor = targetO.GetComponent<ARAnchor>();
        if (arAnchor)
        {
            Destroy(arAnchor);
            Debug.Log("aranchor destroyed");
        }
    }
    public void DeleteTarget()
    {
        _transitionAnimation.MoveToInitialPosition();
    }
    public void ShowTarget()
    {
        _transitionAnimation.MoveToFinalPosition();
    }
    public void EnableARAnchor()
    {
        if (!imageTrackedSelection)
        {
            foreach (KeyValuePair<String,GameObject> valuePair in imageTrackedVisualization._instantiated)
            {
                if (valuePair.Value.TryGetComponent<CalculatePositionAR>(out var calculatePositionAR))
                {
                    if (calculatePositionAR.IsUbicated && !valuePair.Value.gameObject.GetComponent<ARAnchor>())
                    {
                        valuePair.Value.gameObject.AddComponent<ARAnchor>();
                    }
                }
            }
            return;
        }
        foreach (KeyValuePair<String,GameObject> valuePair in imageTrackedSelection._instantiated)
        {
            if (valuePair.Value.TryGetComponent<CalculatePositionAR>(out var calculatePositionAR))
            {
                if (calculatePositionAR.IsUbicated && !valuePair.Value.gameObject.GetComponent<ARAnchor>())
                {
                    valuePair.Value.gameObject.AddComponent<ARAnchor>();
                }
            }
        }
    }
}
