using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadData : MonoBehaviour
{
    [SerializeField] private ObjectsPositionInfo _objectsPositionInfo;
    [SerializeField] private bool useDefault;
    private void Start()
    {
        LoadModelData();
    }

    public void LoadModelData()
    {
        if (useDefault)
        {
            return;
        }
        if (PlayerPrefs.HasKey("Positions"))
        {
            try
            {
                var dataJSON = PlayerPrefs.GetString("Positions");
                var data = JsonUtility.FromJson<ListPositions>(dataJSON);
                _objectsPositionInfo._listPositions._PositionInfos = data._PositionInfos;
                Debug.Log("cargando datos");
                for (int i = 0; i < _objectsPositionInfo._ModelsToInstanciates.Count; i++)
                {
                    var idObject = data._PositionInfos.Exists(x =>
                        x.idObject == _objectsPositionInfo._ModelsToInstanciates[i].idObject);
                    _objectsPositionInfo._ModelsToInstanciates[i].instantiated = idObject;
                }
                _objectsPositionInfo.isSavedInfo = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }
        else
        {
            Debug.Log("limpiar datos");
            _objectsPositionInfo._listPositions._PositionInfos.Clear();
            for (int i = 0; i < _objectsPositionInfo._ModelsToInstanciates.Count; i++)
            {
                _objectsPositionInfo._ModelsToInstanciates[i].instantiated = false;
            }
            _objectsPositionInfo.isSavedInfo = false;
        }
    }
}
