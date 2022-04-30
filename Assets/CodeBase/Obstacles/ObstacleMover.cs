using DG.Tweening;
using UnityEngine;

namespace CodeBase.Obstacles
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ObstacleMover : MonoBehaviour
    {
        [SerializeField] private Transform _endPoint;
        [SerializeField] private float _moveDuration;
        [SerializeField] private Rigidbody2D _rigidbody;

        public void Initialize()
        {
            SetSettings();
            MoveObstacle();
        }

        private void SetSettings()
        {
            _rigidbody.bodyType = RigidbodyType2D.Kinematic;
            _rigidbody.useFullKinematicContacts = true;
            _rigidbody.interpolation = RigidbodyInterpolation2D.Interpolate;
        }

        private void MoveObstacle() => 
            _rigidbody
                .DOMove(_endPoint.position, _moveDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.Linear);

        public void Disable() => 
            _rigidbody.DOKill();
    }
}
