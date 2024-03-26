using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class InformationManager : MonoBehaviour
{
    [SerializeField] private ObjectInformation info;
    [SerializeField] private GameObject _containerInfoUI;
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _description;
    public void AddObject(ObjectInformation info)
    {
        if (this.info)
        {
            this.info.HideInformation();
        }
        this.info = info;
        _title.text = this.info.ObjectInfoData.title;
        _description.text = this.info.ObjectInfoData.description;
        _containerInfoUI.SetActive(true);
    }

    public void RemoveObject()
    {
        info.HideInformation();
        _containerInfoUI.SetActive(false);
        info = null;
    }
}
