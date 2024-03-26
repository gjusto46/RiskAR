using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DragValue : MonoBehaviour, IPointerMoveHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private DirectionDrag _directionDrag;
    private Vector2 _initialPosition;
    private Vector2 _actualPosition;
    private bool _isSelected;
    private float valueDrag;
    public UnityEvent<float> onChangeValue;
    private Vector2 _difference;
    enum DirectionDrag
    {
        Vertical,
        Horizontal
    }
    public void InitialPosition()
    {
        // _initialPosition = Pointer.current.delta.value;
        _initialPosition = Vector2.zero;
    }

    public void UpdatePosition()
    {
        if (_isSelected)
        {
            _actualPosition = Pointer.current.delta.value;
            _difference = _actualPosition - _initialPosition; 
            if (_directionDrag == DirectionDrag.Horizontal)
            {
                onChangeValue?.Invoke(_difference.x);
            }
            else
            {
                onChangeValue?.Invoke(_difference.y);
            }
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
       UpdatePosition();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isSelected = true;
        InitialPosition();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isSelected = false;
    }
}
