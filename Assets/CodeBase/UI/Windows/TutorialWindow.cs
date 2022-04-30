using System.Collections;
using CodeBase.Infrastructure.Services.Input;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Windows
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

        private Quaternion _handImageRotation;
        private Vector3 _handImagePosition;
        private Vector3 _touchImagePosition;
        private Color _touchImageColor;
        private IInputService _inputService;

        [Inject]
        private void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }

        public void StartTutorial()
        {
            SaveImagesStartState();
            _inputService.Released += OnFrogJumped; 

            StartCoroutine(StartAnimation());
        }

        private IEnumerator StartAnimation()
        {
            //yield return new WaitForSeconds(0.3f);
            _canvasGroup.DOFade(1f, 0.5f);
            yield return new WaitForSeconds(0.5f);
            StartAnim();
        }

        private void SaveImagesStartState()
        {
            _handImageRotation = _handImage.transform.rotation;
            _handImagePosition = _handImage.transform.position;
            _touchImagePosition = _touchImage.transform.position;
            _touchImageColor = _touchImage.color;
        }

        public void Open() => 
            gameObject.SetActive(true);

        private void Close() => 
            gameObject.SetActive(false);

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

        private void OnFrogJumped(Vector2 _)
        {
            KillAnimations();
            ResetImagesStates();
            _inputService.Released -= OnFrogJumped; 
            _canvasGroup.alpha = 0f;
            Close();
            
            Destroy(gameObject);
        }

        private void KillAnimations()
        {
            _handSequence.Kill();
            _touchSequence.Kill();
        }

        private void ResetImagesStates()
        {
            Transform handImageTransform = _handImage.transform;
            handImageTransform.rotation = _handImageRotation;
            handImageTransform.position = _handImagePosition;
            
            _touchImage.transform.position = _touchImagePosition;
            _touchImage.color = _touchImageColor;
        }
        
        public class Factory : PlaceholderFactory<TutorialWindow>
        {
        }
    }
}
