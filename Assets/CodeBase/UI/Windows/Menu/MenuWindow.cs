using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows.Menu
{
    public class MenuWindow : BaseWindow, ISavedProgressReader
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private Image _levelProgressBar;
        [SerializeField] private TMP_Text _levelProgressText;
        [SerializeField] private Button _shopButton;
        [SerializeField] private Button _settingsButton;

        private int _currentLevelIndex;
        private int _levelsCount;

        public event Action PlayClicked;
        public event Action ShopClicked;
        public event Action SettingsClicked;

        protected override void Initialize() => 
            UpdateLevelProgress();

        protected override void SubscribeUpdates()
        {
            _playButton.onClick.AddListener(OnPlayClicked);
            _shopButton.onClick.AddListener(OnShopClicked);
            _settingsButton.onClick.AddListener(OnSettingsClicked);
        }

        private void OnPlayClicked() => 
            PlayClicked?.Invoke();

        private void OnSettingsClicked()
        {
            SettingsClicked?.Invoke();
            gameObject.SetActive(false);
        }

        private void OnShopClicked()
        {
            ShopClicked?.Invoke();
            gameObject.SetActive(false);
        }

        private void UpdateLevelProgress()
        {
            _levelProgressBar.fillAmount = (float)(_currentLevelIndex) / _levelsCount;
            _levelProgressText.text = $"{_currentLevelIndex}/{_levelsCount}";
        }

        protected override void Cleanup()
        {
            _playButton.onClick.RemoveListener(OnPlayClicked);
            _shopButton.onClick.RemoveListener(OnShopClicked);
            _settingsButton.onClick.RemoveListener(OnSettingsClicked);
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _currentLevelIndex = progress.PlayerData.currentLevelIndex;
            _levelsCount = progress.PlayerData.levelsCount;
        }
    }
}
