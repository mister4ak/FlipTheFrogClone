using CodeBase.Infrastructure.Services.PersistentProgress;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Windows.Game
{
    public class EndLevelWindow : BaseWindow
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TMP_Text _currentLevelText;
        private PersistentProgressService _progressService;

        [Inject]
        private void Construct(PersistentProgressService progressService) => 
            _progressService = progressService;

        protected override void Initialize()
        {
            SetCompletedLevelText();
            _canvasGroup.alpha = MinAlpha;
            _canvasGroup.DOFade(MaxAlpha, FadeDuration);
        }

        private void SetCompletedLevelText() => 
            _currentLevelText.text = _progressService.Progress.PlayerData.currentLevelIndex.ToString();
    }
}
