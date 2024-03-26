using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToInformationManager : MonoBehaviour
{
    private InformationManager _informationManager;

    public void AddToManager(ObjectInformation info)
    {
        if (!_informationManager)
        {
            _informationManager = FindObjectOfType<InformationManager>();
        }
        _informationManager.AddObject(info);
    }

    public void RemoveTomanager()
    {
        _informationManager.RemoveObject();
    }
}
