using Unity.XR.CoreUtils;
using UnityEngine;

namespace GenericScripts
{
    public class LookAtCamera : MonoBehaviour
    {
        private Camera _camera;
        private void Start()
        {
            _camera = FindObjectOfType<XROrigin>().Camera;
        
        }

        private void Update()
        {
            transform.LookAt(_camera.transform.position);
            var rotationTemp = transform.eulerAngles;
            rotationTemp.x = 0;
            rotationTemp.z = 0;
            rotationTemp.y += 180;
            transform.eulerAngles = rotationTemp;
        }
    }
}
