using System;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows
{
    public class PauseWindow : WindowBase
    {
        [SerializeField] private Button _mainMenuButton;

        public event Action PlayButtonClicked;
        public event Action MenuButtonClicked;

        protected override void SubscribeUpdates()
        {
            _closeButton.onClick.AddListener(OnCloseButtonClicked);
            _mainMenuButton.onClick.AddListener(OnMenuButtonClicked);
        }

        private void OnMenuButtonClicked()
        {
            MenuButtonClicked?.Invoke();
            gameObject.SetActive(false);
        }

        private void OnCloseButtonClicked()
        {
            PlayButtonClicked?.Invoke();
            gameObject.SetActive(false);
        }

        protected override void Cleanup()
        {
            _closeButton.onClick.RemoveListener(OnCloseButtonClicked);
            _mainMenuButton.onClick.RemoveListener(OnMenuButtonClicked);
        }
    }
}
