using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ObjectInformation : MonoBehaviour
{
    [SerializeField] private ObjectInfoData _objectInfoData;
    [SerializeField] private Toggle _toggle;

    public ObjectInfoData ObjectInfoData
    {
        get => _objectInfoData;
        set => _objectInfoData = value;
    }
    public void ShowInformation()
    {
        _toggle.isOn = true;
    }

    public void HideInformation()
    {
        _toggle.isOn = false;

    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "ImageTraking Creacion")
        {
            gameObject.SetActive(false);
        }
    }
}
