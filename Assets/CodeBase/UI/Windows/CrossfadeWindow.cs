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

        public event Action CrossfadeEnded;

        private void Awake() => 
            DontDestroyOnLoad(gameObject);

        public void Open()
        {
            gameObject.SetActive(true);
            _canvasGroup.alpha = _minAlpha;
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.DOFade(_maxAlpha, _fadeDuration)
                .SetUpdate(true)
                .OnComplete(() => CrossfadeEnded?.Invoke());
        }

        public void Close()
        {
            _canvasGroup.alpha = _maxAlpha;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.DOFade(_minAlpha, _fadeDuration)
                .SetUpdate(true)
                .OnComplete(OnCrossfadeEnded);
        }

        private void OnCrossfadeEnded()
        {
            CrossfadeEnded?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
