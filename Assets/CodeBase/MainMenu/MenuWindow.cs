using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.MainMenu
{
    public class MenuWindow : MonoBehaviour, ISavedProgressReader
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

        public void Initialize()
        {
            Subscribe();
            UpdateLevelProgress();
        }

        private void Subscribe()
        {
            _playButton.onClick.AddListener(() => PlayClicked?.Invoke());
            _shopButton.onClick.AddListener(() => ShopClicked?.Invoke());
            _settingsButton.onClick.AddListener(() => SettingsClicked?.Invoke());
        }

        private void UpdateLevelProgress()
        {
            _levelProgressBar.fillAmount = (float)(_currentLevelIndex) / _levelsCount;
            _levelProgressText.text = $"{_currentLevelIndex}/{_levelsCount}";
        }

        public void Open() => 
            gameObject.SetActive(true);

        public void Close() => 
            gameObject.SetActive(false);

        public void LoadProgress(PlayerProgress progress)
        {
            _currentLevelIndex = progress.PlayerData.currentLevelIndex;
            _levelsCount = progress.PlayerData.levelsCount;
        }
    }
}
