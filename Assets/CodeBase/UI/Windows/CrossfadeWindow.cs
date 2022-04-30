using System;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.UI.Windows
{
    public class CrossfadeWindow : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        private const float _maxAlpha = 1f;
        private const float _minAlpha = 0f;
        private const float _fadeDuration = 0.5f;

        private void Awake() => 
            DontDestroyOnLoad(gameObject);

        public void Open(Action onCrossfadeEnded = null)
        {
            gameObject.SetActive(true);
            _canvasGroup.alpha = _minAlpha;
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.DOFade(_maxAlpha, _fadeDuration)
                .SetUpdate(true)
                .OnComplete(() => onCrossfadeEnded?.Invoke());
        }

        public void Close(Action onCrossfadeEnded = null)
        {
            _canvasGroup.alpha = _maxAlpha;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.DOFade(_minAlpha, _fadeDuration)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    onCrossfadeEnded?.Invoke();
                    gameObject.SetActive(false);
                });
        }
    }
}
