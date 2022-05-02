using System;
using CodeBase.Colliders;
using CodeBase.Data;
using CodeBase.Environment;
using CodeBase.Environment.Obstacles;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Frog
{
    [RequireComponent(typeof(FrogMover))]
    public class FrogPlayer : MonoBehaviour, ISavedProgressReader
    {
        [SerializeField] private FrogMover _frogMover;
        [SerializeField] private FrogArrow _frogArrow;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private TrailRenderer _frogTrail;
        private string _skinPath;
        private bool _died;

        public event Action Jumped;
        public event Action Died;
        public event Action<Vector2> ObstacleCollided;

        public void SetStartPosition(Vector2 position)
        {
            _frogArrow.SetStartPosition(position);
            _frogMover.SetStartPosition(position);
        }

        public void SetHoldPosition(Vector2 position) => 
            _frogArrow.SetHoldArrowPosition(position);

        public void SetReleasedPosition(Vector2 position)
        {
            _frogArrow.SetActiveState(false);
            _frogMover.SetReleasedPosition(position);
            Jumped?.Invoke();
        }

        public void Disable() => 
            _frogArrow.SetActiveState(false);

        public void Move(Vector2 from, Vector2 to, float duration)
        {
            _frogMover.ResetVelocity();
            SetPosition(from);
            _frogMover.MoveTo(to, duration);
        }

        private void SetFrogTrailState(bool state) => 
            _frogTrail.enabled = state;

        private void SetPosition(Vector2 position) => 
            transform.position = position;

        public void ResetFrog(Vector2 startPosition)
        {
            SetFrogTrailState(false);
            SetPosition(startPosition);
            SetFrogTrailState(true);
            
            _frogMover.ResetVelocity();
            gameObject.SetActive(true);
            _died = false;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out DeadZone deadZone) 
                || collision.gameObject.TryGetComponent(out Spike spike) 
                && _died == false)
            {
                _died = true;
                Died?.Invoke();
            }
            else if (collision.gameObject.TryGetComponent(out Obstacle obstacle) || collision.gameObject.TryGetComponent(out Border border))
                ObstacleCollided?.Invoke(collision.GetContact(0).point);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Trampoline trampoline))
            {
                SetPosition(trampoline.GetJumpPosition());
                _frogMover.JumpFromTrampoline(trampoline);
            }
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _skinPath = progress.SkinData.skinSpritePath;
            ChangeSkin();
        }

        private void ChangeSkin()
        {
            Sprite skin = Resources.Load<Sprite>(_skinPath);
            _spriteRenderer.sprite = skin;
        }
    }
}
