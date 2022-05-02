using System.Collections;
using CodeBase.Infrastructure.Services.Input;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Windows.Game
{
    public class TutorialWindow : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Image _handImage;
        [SerializeField] private Image _touchImage;
    
        [SerializeField] private float _startDelay;
        [SerializeField] private float _handRotationX;
        [SerializeField] private float _handRotationDuration;

        [SerializeField] private float _touchFadeDuration;

        [SerializeField] private float _movePositionY;
        [SerializeField] private float _moveDuration;
        [SerializeField] private float _handSecondRotationX;
        [SerializeField] private float _touchDelay;
        [SerializeField] private float _finishFadeDuration;
        
        private Sequence _handSequence;
        private Sequence _touchSequence;
        private IInputService _inputService;
        private Vector2 _clickedPosition;
        private Vector2 _releasedPosition;
        private float _minDistance = 50f;

        [Inject]
        private void Construct(IInputService inputService) => 
            _inputService = inputService;

        public void StartTutorial()
        {
            _inputService.Clicked += SaveClickedPosition;
            _inputService.Released += OnFrogJumped;
            StartCoroutine(StartAnimation());
        }

        private void SaveClickedPosition(Vector2 clickedPosition) => 
            _clickedPosition = clickedPosition;

        private IEnumerator StartAnimation()
        {
            _canvasGroup.DOFade(1f, 0.5f);
            yield return new WaitForSeconds(0.5f);
            StartAnim();
        }

        public void Open() => 
            gameObject.SetActive(true);

        private void StartAnim()
        {
            AnimateHandImage();
            AnimateTouchImage();
        }

        private void AnimateHandImage()
        {
            _handSequence = DOTween.Sequence()
                .AppendInterval(_startDelay)
                .Append(_handImage.transform.DORotate(new Vector3(_handRotationX, 0f, 0f), _handRotationDuration)
                    .SetEase(Ease.Linear))
                .AppendInterval(_handRotationDuration + _touchFadeDuration)
                .Append(_handImage.transform.DOMoveY(_handImage.transform.position.y - _movePositionY, _moveDuration))
                .AppendInterval(_touchDelay)
                .Append(_handImage.transform.DORotate(new Vector3(_handSecondRotationX, 0f, 0f), _handRotationDuration)
                    .SetEase(Ease.Linear))
                .AppendInterval(0.3f)
                .Append(_handImage.DOFade(0f, _finishFadeDuration))
                .AppendInterval(0.5f)
                .SetLoops(-1, LoopType.Restart).SetUpdate(true);
        }

        private void AnimateTouchImage()
        {
            _touchSequence = DOTween.Sequence()
                .AppendInterval(_handRotationDuration + _startDelay)
                .Append(_touchImage.DOFade(1f, _touchFadeDuration))
                .AppendInterval(_handRotationDuration)
                .Append(_touchImage.transform.DOMoveY(_touchImage.transform.position.y - _movePositionY, _moveDuration))
                .AppendInterval(_touchDelay)
                .Append(_touchImage.DOFade(0f, _handRotationDuration))
                .AppendInterval(0.8f + _finishFadeDuration)
                .SetLoops(-1, LoopType.Restart).SetUpdate(true);
        }

        private void OnFrogJumped(Vector2 releasedPosition)
        {
            _releasedPosition = releasedPosition;
            if (ReleasedPositionSameAsClicked())
                return;
            
            KillAnimations();
            _inputService.Released -= OnFrogJumped;
            Destroy(gameObject);
        }

        private bool ReleasedPositionSameAsClicked() => 
            Vector2.Distance(_clickedPosition, _releasedPosition) < _minDistance;

        private void KillAnimations()
        {
            _handSequence.Kill();
            _touchSequence.Kill();
        }

        public class Factory : PlaceholderFactory<TutorialWindow>
        {
        }
    }
}
