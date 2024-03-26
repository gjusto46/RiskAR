using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleInteraction : MonoBehaviour
{
    private Toggle _toggle;
    [SerializeField] private UnityEvent _on;
    [SerializeField] private UnityEvent _off;
    
    private void Start()
    {
        _toggle = GetComponent<Toggle>();
        _toggle.onValueChanged.AddListener(Call);
    }

    private void Call(bool arg0)
    {
        if (arg0)
        {
            _on?.Invoke();
        }
        else
        {
            _off?.Invoke();
        }
    }
}
