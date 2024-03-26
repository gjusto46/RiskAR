using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class TransitionAnimation : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;

    [ContextMenu("move final")]
    public void MoveToFinalPosition()
    {
        DOTween.KillAll();
        _canvasGroup.interactable = true;
        _canvasGroup.DOFade(1, .2f);
    }
    [ContextMenu("move initial")]
    public void MoveToInitialPosition()
    {
        DOTween.KillAll();
        _canvasGroup.interactable = false;
        _canvasGroup.DOFade(0, .2f);
    }

    public void Change(bool value)
    {
        if (value)
        {
            MoveToFinalPosition();
        }
        else
        {
            MoveToInitialPosition();
        }
    }
}
