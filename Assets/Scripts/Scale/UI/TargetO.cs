using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

namespace UI
{
    public class TargetO : MonoBehaviour
    {
        [SerializeField] private Slider _sliderScale;
        [SerializeField] private GameObject _objectToScale;
        [SerializeField] private bool _haveAnimationRigging;
        [SerializeField] private List<RigBuilder> _rigBuilders;
        [SerializeField] private Transform _transformReference;
        [SerializeField] private ARAnchor _arAnchor;
        [SerializeField] private GameObject _renderMarker;
        [SerializeField] private bool _isVertical;
        private Vector3 _initialPosition, _initialScale;
        private Quaternion _initialQuaternion;
        private bool _isCreationScene;

        public bool IsVertical => _isVertical;
        private void Awake()
        {
            if (SceneManager.GetActiveScene().name == "ImageTracking Creacion")
            {
                _isCreationScene = true;
            }
            else
            {
                _isCreationScene = false;
            }
        }

        public Vector3 InitialPosition
        {
            get => _initialPosition;
            set => _initialPosition = value;
        }

        public Quaternion InitialQuaternion
        {
            get => _initialQuaternion;
            set => _initialQuaternion = value;
        }

        public Vector3 InitialScale
        {
            get => _initialScale;
            set => _initialScale = value;
        }
        public ARAnchor ARAnchor
        {
            get => _arAnchor;
            set => _arAnchor = value;
        }
        public Transform TransformReference
        {
            get => _transformReference;
            set => _transformReference = value;
        }
        private void OnEnable()
        {
            if (_isCreationScene)
            {   
                return;
            }
            _sliderScale = FindObjectOfType<Slider>(true);
            _sliderScale.value = Mathf.Clamp(_objectToScale.transform.localScale.x, _sliderScale.minValue, _sliderScale.maxValue); 
            _sliderScale.onValueChanged.AddListener(ChangeScale);
        }

        private void OnDisable()
        {
            if (_isCreationScene)
            {   
                return;
            }
            _sliderScale.onValueChanged.RemoveListener(ChangeScale);
        }

        private void OnDestroy()
        {
            Debug.Log("Objeto destruido");
        }

        private void ChangeScale(float arg0)
        {
            if (_haveAnimationRigging)
            {
                UpdateRigState(false);
            }
            _objectToScale.transform.localScale = Vector3.one * arg0;
            if (_haveAnimationRigging)
            {
                UpdateRigState(true);

            }
        }

        public void UpdateRigState(bool state)
        {
            for (int i = 0; i < _rigBuilders.Count; i++)
            {
                _rigBuilders[i].enabled = state;
            }
        }

      
        public void HideRenderMarker()
        {
            _renderMarker.SetActive(false);
        }
        public void ShowRenderMarker()
        {
            _renderMarker.SetActive(true);
        }

        // private void OnDrawGizmos()
        // {
        //     Gizmos.color = Color.red;
        //     Gizmos.DrawRay(transform.position, transform.right);
        //     Gizmos.color = Color.green;
        //     Gizmos.DrawRay(transform.position, transform.up);
        //     Gizmos.color = Color.blue;
        //     Gizmos.DrawRay(transform.position, transform.forward);
        // }
    }
}
