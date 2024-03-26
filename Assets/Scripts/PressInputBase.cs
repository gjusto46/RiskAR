using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PressInputBase : MonoBehaviour
{
    protected InputAction _pressAction;

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

    protected virtual void OnPress(Vector3 position) {}

    protected virtual void OnPressBegan(Vector3 position) {}

    protected virtual void OnPressCancel() {}
}