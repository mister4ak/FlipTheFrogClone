using System;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.UI.Windows.Game
{
    public class CrossfadeWindow : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        private const float MaxAlpha = 1f;
        private const float MinAlpha = 0f;
        private const float FadeDuration = 0.5f;

        private void Awake() => 
            DontDestroyOnLoad(gameObject);

        public void Open(Action onCrossfadeEnded = null)
        {
            gameObject.SetActive(true);
            _canvasGroup.alpha = MinAlpha;
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.DOFade(MaxAlpha, FadeDuration)
                .SetUpdate(true)
                .OnComplete(() => onCrossfadeEnded?.Invoke());
        }

        public void Close(Action onCrossfadeEnded = null)
        {
            _canvasGroup.alpha = MaxAlpha;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.DOFade(MinAlpha, FadeDuration)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    onCrossfadeEnded?.Invoke();
                    gameObject.SetActive(false);
                });
        }
    }
}
