using DG.Tweening;
using UnityEngine;

namespace CodeBase.UI.Windows.Game
{
    public class GameOverWindow : WindowBase
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        protected override void Initialize()
        {
            _canvasGroup.alpha = MinAlpha;
            _canvasGroup.DOFade(MaxAlpha, FadeDuration);
        }
    }
}
