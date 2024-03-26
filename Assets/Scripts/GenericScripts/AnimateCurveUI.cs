using System;
using UnityEngine;

namespace GenericScripts
{
    public class AnimateCurveUI : MonoBehaviour
    {
        [SerializeField] private AnimationCurve _animationCurve;
        private float _actualTime;
        private float _valueCurve;
        private Vector3 _scale;
        private void Start()
        {
            _scale = transform.localScale;
        }

        private void Update()
        {
            _actualTime += Time.deltaTime;
            _actualTime %= 1;
            _valueCurve = _animationCurve.Evaluate(_actualTime);
            transform.localScale = _scale * _valueCurve;
        }
    }
}
