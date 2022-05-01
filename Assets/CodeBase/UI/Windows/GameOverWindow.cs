using System;

namespace CodeBase.UI.Windows
{
    public class GameOverWindow : WindowBase
    {
        public event Action RestartButtonClicked;

        protected override void SubscribeUpdates() => 
            _closeButton.onClick.AddListener(OnRestartButtonClicked);

        private void OnRestartButtonClicked()
        { 
            RestartButtonClicked?.Invoke();
            gameObject.SetActive(false);
        }

        protected override void Cleanup() => 
            _closeButton.onClick.RemoveListener(OnRestartButtonClicked);
    }
}
