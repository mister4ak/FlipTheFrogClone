using Cinemachine;
using UnityEngine;

namespace CodeBase.CameraLogic
{
    public class CinemachineSwitcher : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _frogCamera;
        [SerializeField] private CinemachineVirtualCamera _moonCamera;
        private bool _isFrogCamera = true;
        public void SwitchState()
        {
            if (_isFrogCamera)
            {
                _frogCamera.Priority = 0;
                _moonCamera.Priority = 1;
            }
            else
            {
                _frogCamera.Priority = 1;
                _moonCamera.Priority = 0;
            }
            _isFrogCamera = !_isFrogCamera;
        }
    }
}
