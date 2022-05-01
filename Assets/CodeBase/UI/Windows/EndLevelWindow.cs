using System;
using CodeBase.Infrastructure.Services.PersistentProgress;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Windows
{
    public class EndLevelWindow : WindowBase
    {
        [SerializeField] private TMP_Text _currentLevelText;
        private PersistentProgressService _progressService;

        public event Action EndLevelScreenClicked;

        [Inject]
        private void Construct(PersistentProgressService progressService) => 
            _progressService = progressService;

        protected override void Initialize() => 
            SetCompletedLevelText();

        private void SetCompletedLevelText() => 
            _currentLevelText.text = _progressService.Progress.PlayerData.currentLevelIndex.ToString();

        protected override void SubscribeUpdates() => 
            _closeButton.onClick.AddListener(OnEndLevelScreenClicked);

        private void OnEndLevelScreenClicked()
        {
            EndLevelScreenClicked?.Invoke();
            gameObject.SetActive(false);
        }

        protected override void Cleanup() => 
            _closeButton.onClick.RemoveListener(OnEndLevelScreenClicked);
    }
}
