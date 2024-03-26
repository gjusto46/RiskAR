using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public enum PHASE
    {
        PlaneDetection,
        ImageDetection,
        MainScene
    }

    [SerializeField] private PHASE _phase;
    [SerializeField] private UnityEvent _onInitState;
    [SerializeField] private UnityEvent _onPlaneDetectionPhase;
    [SerializeField] private UnityEvent _onImageDetectionPhase;
    [SerializeField] private UnityEvent _onMainScene;

    public PHASE Phase
    {
        get => _phase;
        set
        {
            _phase = value;
            switch (_phase)
            {
                case PHASE.ImageDetection:
                    _onImageDetectionPhase?.Invoke();
                    break;
                case PHASE.PlaneDetection:
                    _onPlaneDetectionPhase?.Invoke();
                    break;
                case PHASE.MainScene:
                    _onMainScene?.Invoke();
                    break;
            }
        }
    }

    private void Awake()
    {
        _onInitState?.Invoke();    
    }

    private void Start()
    {
        ChangePhase(PHASE.PlaneDetection);
    }

    public void ChangePhase(PHASE phase)
    {
        Phase = phase;
    }
    public void ChangePhaseToImage()
    {
        StartCoroutine(Delay(PHASE.ImageDetection));
    }

    IEnumerator Delay(PHASE phase)
    {
        yield return new WaitForSeconds(1);
        ChangePhase(phase);
    }
    public void ChangePhaseToMainScene()
    {
        StartCoroutine(Delay(PHASE.MainScene));
    }
}
