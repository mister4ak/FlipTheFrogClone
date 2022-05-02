using System.Collections.Generic;
using System.Linq;
using CodeBase.Infrastructure.Services;
using CodeBase.StaticData;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace CodeBase.Frog
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class FrogMover : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _frogRigidbody;
        [SerializeField] private LayerMask _tutorialLayer;
        private StaticDataService _staticData;
        private PointerEventData _eventData;
        private List<RaycastResult> _raycastResults;
        
        private Vector2 _clickedPosition;
        private Vector2 _releasedPosition;
        private float _maxDragDistance;
        private float _forceLaunch;

        [Inject]
        private void Construct(StaticDataService staticData) => 
            _staticData = staticData;

        private void Start()
        {
            InitStaticData();
            InitEventSystemForUIHit();
        }

        private void InitStaticData()
        {
            PlayerStaticData playerData = _staticData.PlayerData();
            _forceLaunch = playerData.forceLaunch;
            _maxDragDistance = playerData.maxDragDistance;
        }

        private void InitEventSystemForUIHit()
        {
            _eventData = new PointerEventData(EventSystem.current);
            _raycastResults = new List<RaycastResult>();
        }

        public void JumpFromTrampoline(Trampoline trampoline) => 
            Launch(trampoline.GetLaunchDirection(), _staticData.PlayerData().trampolineForceLaunch);

        public void ResetVelocity() => 
            _frogRigidbody.velocity = Vector2.zero;

        public void MoveTo(Vector2 position, float moveDuration)
        {
            _frogRigidbody.isKinematic = true;
            transform
                .DOMoveY(position.y, moveDuration)
                .OnComplete(() =>
                {
                    _frogRigidbody.isKinematic = false;
                    gameObject.SetActive(false);
                });
        }

        public void SetStartPosition(Vector2 clickedPosition) => 
            _clickedPosition = clickedPosition;

        public void SetReleasedPosition(Vector2 releasedPosition)
        {
            if (HitUI())
                return;
            _releasedPosition = releasedPosition;
            Launch(CalculateDirection(), CalculateForceLaunch());
        }

        private bool HitUI()
        {
            _eventData.position = _clickedPosition;
            EventSystem.current.RaycastAll(_eventData, _raycastResults);
            
            if (HitNotTutorialLayer())
                return false;

            return _raycastResults.Count > 0;
        }

        private bool HitNotTutorialLayer() => 
            _raycastResults.Any(raycastResult => _tutorialLayer.value == 1 << raycastResult.gameObject.layer);

        private void Launch(Vector2 direction, float forceLaunch)
        {
            ResetVelocity();
            _frogRigidbody.AddForce(direction * forceLaunch, ForceMode2D.Impulse);
        }

        private Vector2 CalculateDirection() => 
            (_clickedPosition - _releasedPosition).normalized;

        private float CalculateForceLaunch()
        {
            float dragDistance = Vector2.Distance(_clickedPosition, _releasedPosition);
            if (dragDistance > _maxDragDistance)
                return _forceLaunch;
            return _forceLaunch * (dragDistance / _maxDragDistance);
        }
    }
}
