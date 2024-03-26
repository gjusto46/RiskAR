using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class PositionInfo
{
    public string idObject;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;

    public Vector3 globalPosition;
    public Quaternion globalRotation;
    public Vector3 globalScale;
}

[Serializable]
public class ModelsToInstanciate
{
    public string idObject;
    public GameObject prefab;
    public bool instantiated;
}

[Serializable]
public class ListPositions
{
    public List<PositionInfo> _PositionInfos  = new List<PositionInfo>();

}
[CreateAssetMenu(fileName = "PositionInfo", menuName = "Position Info")]
public class ObjectsPositionInfo : ScriptableObject
{
    public bool isSavedInfo;
    public List<ModelsToInstanciate> _ModelsToInstanciates;
    public ListPositions _listPositions;
    public void ClearList()
    {
        isSavedInfo = false;
        _listPositions._PositionInfos.Clear();
        Debug.Log("Lista limpia");
    }
}
