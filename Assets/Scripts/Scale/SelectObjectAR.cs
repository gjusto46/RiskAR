using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SelectObjectAR : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float _maxTimeInteraction;
    [SerializeField] private UnityEvent _onSelect;
    [SerializeField] private UnityEvent _onSelectInVisualizationScene;
    private float _timeDrag;
    private bool _isPress;
    public void OnPointerDown(PointerEventData eventData)
    {
        _isPress = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isPress = false;
        if (_timeDrag <= _maxTimeInteraction )
        {
            _onSelect?.Invoke();
        }
        _timeDrag = 0;
    }

    private void Update()
    {
        if (_isPress)
        {
            _timeDrag += Time.deltaTime;
        }
    }
}
