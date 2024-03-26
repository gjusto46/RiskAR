using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class CreateInCloudPoint : MonoBehaviour
{
    [SerializeField] private ARPlaneManager _arPointCloudManager;
    [SerializeField] private ARRaycastManager _arRaycastManager;
    [SerializeField] private GameObject _prefabToInstantiate;
    private List<GameObject> _prefabInstantiated = new List<GameObject>();
    private List<ARRaycastHit> _arRaycastHits = new List<ARRaycastHit>();
    private List<GenerateWallProcedeural> _walls = new List<GenerateWallProcedeural>();
    protected InputAction _pressAction;
    [SerializeField] bool _pressed;
    [SerializeField] bool _instantiated;
    [SerializeField] private LineRenderer _lineRenderer;
    private int _oldCountElements;
    private bool _isUnicClick;
    protected virtual void Awake()
    {
        _pressAction = new InputAction("touch", binding: "<Pointer>/press");

        _pressAction.started += ctx =>
        {
            if (ctx.control.device is Pointer device)
            {
                OnPressBegan(device.position.ReadValue());
            }
        };

        _pressAction.performed += ctx =>
        {
            if (ctx.control.device is Pointer device)
            {
                OnPress(device.position.ReadValue());
            }
        };

        _pressAction.canceled += _ => OnPressCancel();
    }

    protected virtual void OnEnable()
    {
        _pressAction.Enable();
    }

    protected virtual void OnDisable()
    {
        _pressAction.Disable();
    }

    protected virtual void OnDestroy()
    {
        _pressAction.Dispose();
    }

    protected virtual void OnPress(Vector3 position)
    {
        // _pressed = true;
        // StartCoroutine(WaitEndFrame());
    }

    IEnumerator WaitEndFrame(Vector2 position)
    {
        
        yield return new WaitForEndOfFrame();

        if (!ARInteractionsManager.isTapOverUI(position))
        {
            _oldCountElements++;
        }
    }

    protected virtual void OnPressBegan(Vector3 position)
    {
        StartCoroutine(WaitEndFrame(position));
        
    }

    protected virtual void OnPressCancel()
    {
        _pressed = false;
        
    }
    
    private void LateUpdate()
    {
        Vector2 middlePositionScreen = new Vector2();
        middlePositionScreen.x = Screen.width / 2; 
        middlePositionScreen.y = Screen.height / 2;
        if (Pointer.current != null && _pressed && !EventSystem.current.IsPointerOverGameObject())
        {
            _oldCountElements++;
            // _prefabInstantiated = null;
        }
        if (_oldCountElements != _prefabInstantiated.Count )
        {
            // Hay mas o menos objetos en la lista que los ya posicionados
            Debug.Log("diferentes");
            _instantiated = true;
        }
        else
        {
            Debug.Log("iguales");
            _instantiated = false;
        }

        
        if (_arRaycastManager.Raycast(middlePositionScreen, _arRaycastHits, TrackableType.Planes))
        {
            // No se cre{o posicion de objeto
            var arHit = _arRaycastHits[0].pose;
            if (!_instantiated)
            {
                _prefabInstantiated.Add(Instantiate(_prefabToInstantiate, arHit.position, arHit.rotation));
                _lineRenderer.positionCount++;
                _lineRenderer.SetPosition(_prefabInstantiated.Count -1, arHit.position);
                if (_oldCountElements > 1)
                {
                    var test = new GameObject();
                    var s = test.AddComponent<GenerateWallProcedeural>();
                    test.layer = LayerMask.NameToLayer("Wall");
                    s.CreateQuad(_prefabInstantiated[^3].transform.position, _prefabInstantiated[^2].transform.position);
                    _walls.Add(s);
                }
            }
            else
            {
                _prefabInstantiated[^1].transform.position = arHit.position;
                _prefabInstantiated[^1].transform.rotation = arHit.rotation;
                _lineRenderer.SetPosition(_prefabInstantiated.Count -1, arHit.position);
            }
        }
        
        
    }

    public void CleanElements()
    {
        _lineRenderer.positionCount = 0;
        for (int i = 0; i < _prefabInstantiated.Count; i++)
        {
            Destroy(_prefabInstantiated[i].gameObject);
        }
        for (int i = 0; i < _walls.Count; i++)
        {
            Destroy(_walls[i].gameObject);
        }

        _oldCountElements = 0;
        _prefabInstantiated.Clear();
        _walls.Clear();
    }
        
}
    
