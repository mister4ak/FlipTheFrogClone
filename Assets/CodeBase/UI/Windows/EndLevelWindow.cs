using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Windows
{
    public class EndLevelWindow : WindowBase
    {
        //[SerializeField] private Button _nextLevelButton;
        [SerializeField] private TMP_Text _currentLevelText;
        private PersistentProgressService _progressService;

        public event Action EndLevelScreenClicked;

        [Inject]
        private void Construct(PersistentProgressService progressService) => 
            _progressService = progressService;

        protected override void Initialize() => 
            SetCompletedLevelText();

        protected override void SubscribeUpdates() => 
            CloseButton.onClick.AddListener(OnEndLevelScreenClicked);

        protected override void Cleanup() => 
            CloseButton.onClick.RemoveListener(OnEndLevelScreenClicked);

        private void OnEndLevelScreenClicked()
        {
            EndLevelScreenClicked?.Invoke();
            gameObject.SetActive(false);
        }

        private void SetCompletedLevelText() => 
            _currentLevelText.text = _progressService.Progress.PlayerData.currentLevelIndex.ToString();
    }
    // public class EndLevelWindow : MonoBehaviour, ISavedProgressReader
    // {
    //     [SerializeField] private Button _nextLevelButton;
    //     [SerializeField] private TMP_Text _currentLevelText;
    //     private int _currentLevelIndex;
    //
    //     public event Action EndLevelScreenClicked;
    //
    //     public void Open()
    //     {
    //         gameObject.SetActive(true);
    //         SetCompletedLevelText();
    //         _nextLevelButton.onClick.AddListener(OnEndLevelScreenClicked);
    //     }
    //
    //     public void Close()
    //     {
    //         gameObject.SetActive(false);
    //         _nextLevelButton.onClick.RemoveListener(OnEndLevelScreenClicked);
    //     }
    //
    //     private void OnEndLevelScreenClicked() => 
    //         EndLevelScreenClicked?.Invoke();
    //
    //     private void SetCompletedLevelText() => 
    //         _currentLevelText.text = _currentLevelIndex.ToString();
    //
    //     public void LoadProgress(PlayerProgress progress) => 
    //         _currentLevelIndex = progress.PlayerData.currentLevelIndex;
    // }
}
