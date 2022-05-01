using System;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows
{
    public class GameOverWindow : WindowBase
    {
        public event Action RestartButtonClicked;

        protected override void SubscribeUpdates() => 
            CloseButton.onClick.AddListener(OnRestartButtonClicked);

        private void OnRestartButtonClicked()
        {
            RestartButtonClicked?.Invoke();
            gameObject.SetActive(false);
        }

        protected override void Cleanup() => 
            CloseButton.onClick.RemoveListener(OnRestartButtonClicked);
    }
    // public class GameOverWindow : MonoBehaviour
    // {
    //     [SerializeField] private Button _restartButton;
    //     public event Action RestartButtonClicked;
    //
    //     public void Open()
    //     {
    //         gameObject.SetActive(true);
    //         _restartButton.onClick.AddListener(OnRestartButtonClicked);
    //     }
    //
    //     public void Close()
    //     {
    //         gameObject.SetActive(false);
    //         _restartButton.onClick.RemoveListener(OnRestartButtonClicked);
    //     }
    //
    //     private void OnRestartButtonClicked() => 
    //         RestartButtonClicked?.Invoke();
    // }
}
