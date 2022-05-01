using System;

namespace CodeBase.UI.Windows
{
    public class TestWindow: WindowBase
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
}