using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CodeBase.UI.Windows.Game
{
    public class EndLevelWindow : BaseWindow, ISavedProgressReader
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TMP_Text _currentLevelText;
        private int _currentLevelIndex;

        protected override void Initialize()
        {
            SetCompletedLevelText();
            _canvasGroup.alpha = MinAlpha;
            _canvasGroup.DOFade(MaxAlpha, FadeDuration);
        }

        private void SetCompletedLevelText() => 
            _currentLevelText.text = $"Level {_currentLevelIndex.ToString()} passed";

        public void LoadProgress(PlayerProgress progress) => 
            _currentLevelIndex = progress.PlayerData.currentLevelIndex;
    }
}
