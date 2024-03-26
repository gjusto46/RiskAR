using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class FadeAnimation : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    public void FadeIn()
    {
        _canvasGroup.DOFade(1, 1);
    }

    public void FadeOut()
    {
        _canvasGroup.DOFade(0, .5f);
    }
}
