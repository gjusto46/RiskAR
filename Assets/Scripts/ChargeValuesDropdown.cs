using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChargeValuesDropdown : MonoBehaviour
{
    [SerializeField] private ObjectsPositionInfo _objectsPositionInfo;
    [SerializeField] private TMP_Dropdown _dropdown;
    public void ChargeValues()
    {
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        options.Add(new TMP_Dropdown.OptionData("Ninguno"));
        for (int i = 0; i < _objectsPositionInfo._ModelsToInstanciates.Count; i++)
        {
            if (!_objectsPositionInfo._ModelsToInstanciates[i].instantiated)
            {
                TMP_Dropdown.OptionData optionData =
                    new TMP_Dropdown.OptionData(_objectsPositionInfo._ModelsToInstanciates[i].prefab.name);
                options.Add(optionData);
            }
            
        }

        _dropdown.options = options;
    }
}
