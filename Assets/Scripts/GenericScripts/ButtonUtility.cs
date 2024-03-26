using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonUtility : MonoBehaviour
{
    [SerializeField] private UnityEvent<bool> _haveChanged;
    [SerializeField] private bool _isActive;

    public void PressButton()
    {
        _isActive = !_isActive;
        _haveChanged?.Invoke(_isActive);
    }
}
