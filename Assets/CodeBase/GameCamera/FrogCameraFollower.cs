using Cinemachine;
using CodeBase.Frog;
using UnityEngine;
using Zenject;

namespace CodeBase.GameCamera
{
    public class FrogCameraFollower : MonoBehaviour
    {
        [SerializeField] private Transform _cameraStartPosition;
        private CinemachineVirtualCamera _frogCamera;
        private FrogPlayer _frogPlayer;

        [Inject]
        private void Construct(FrogPlayer frogPlayer) => 
            _frogPlayer = frogPlayer;

        private void Awake()
        {
            _frogCamera = GetComponent<CinemachineVirtualCamera>();
            Follow();
        }

        private void Follow() => 
            _frogCamera.Follow = _frogPlayer.transform;

        public void ResetPosition() => 
            _frogCamera.ForceCameraPosition(_cameraStartPosition.position, Quaternion.identity);
    }
}
