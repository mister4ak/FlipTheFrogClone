using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows
{
    public class EndLevelWindow : MonoBehaviour, ISavedProgressReader
    {
        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private TMP_Text _currentLevelText;
        private int _currentLevelIndex;

        public event Action EndLevelScreenClicked;

        public void Open()
        {
            gameObject.SetActive(true);
            SetCompletedLevelText();
            _nextLevelButton.onClick.AddListener(OnEndLevelScreenClicked);
        }

        public void Close()
        {
            gameObject.SetActive(false);
            _nextLevelButton.onClick.RemoveListener(OnEndLevelScreenClicked);
        }

        private void OnEndLevelScreenClicked() => 
            EndLevelScreenClicked?.Invoke();

        private void SetCompletedLevelText() => 
            _currentLevelText.text = _currentLevelIndex.ToString();

        public void LoadProgress(PlayerProgress progress) => 
            _currentLevelIndex = progress.PlayerData.currentLevelIndex;
    }
}
