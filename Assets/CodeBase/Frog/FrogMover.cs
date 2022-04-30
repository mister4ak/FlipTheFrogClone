using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Audio;
using CodeBase.Infrastructure.Services.StaticData;
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
        private ParticleEmmiter _particleEmmiter;
        private AudioPlayer _audioPlayer;
        
        private Vector2 _clickedPosition;
        private Vector2 _releasedPosition;
        private float _maxDragDistance;
        private float _forceLaunch;
        public event Action Jumped;

        [Inject]
        private void Construct(
            StaticDataService staticData,
            ParticleEmmiter particleEmmiter,
            AudioPlayer audioPlayer)
        {
            _audioPlayer = audioPlayer;
            _particleEmmiter = particleEmmiter;
            _staticData = staticData;
        }

        private void Start()
        {
            InitStaticData();
            InitEventSystemForUIHit();
        }

        private void InitStaticData()
        {
            PlayerStaticData playerData = _staticData.PlayerData();
            _forceLaunch = playerData.ForceLaunch;
            _maxDragDistance = playerData.MaxDragDistance;
        }

        private void InitEventSystemForUIHit()
        {
            _eventData = new PointerEventData(EventSystem.current);
            _raycastResults = new List<RaycastResult>();
        }

        public void JumpFromTrampoline(Trampoline trampoline) => 
            Launch(trampoline.GetLaunchDirection(), _staticData.PlayerData().TrampolineForceLaunch);

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
            if (HitUI(releasedPosition))
                return;
            _releasedPosition = releasedPosition;
            Launch(CalculateDirection(), CalculateForceLaunch());
        }

        private bool HitUI(Vector2 clickedPosition)
        {
            _eventData.position = clickedPosition;
            EventSystem.current.RaycastAll(_eventData, _raycastResults);
            
            if (_raycastResults.Any(raycastResult => _tutorialLayer.value == 1 << raycastResult.gameObject.layer))
                return false;

            return _raycastResults.Count > 0;
        }

        private void Launch(Vector2 direction, float forceLaunch)
        {
            ResetVelocity();
            _frogRigidbody.AddForce(direction * forceLaunch, ForceMode2D.Impulse);
            PlayParticleEffect();
            PlaySoundEffect();
            Jumped?.Invoke();
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

        private void PlayParticleEffect() => 
            _particleEmmiter.Play(Particle.Jump, transform.position);

        private void PlaySoundEffect() => 
            _audioPlayer.Jump();
    }
}
