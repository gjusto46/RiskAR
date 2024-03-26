using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class SelectAddManager : MonoBehaviour
{
    private UpdateTransformManager _updateTransformManager;

    public void AddToManager(TargetO targetO)
    {
        if (!_updateTransformManager)
        {
            _updateTransformManager = FindObjectOfType<UpdateTransformManager>();
        }
        targetO.ShowRenderMarker();
        _updateTransformManager.AddObject(targetO);
        targetO.enabled = true;
    }
}
