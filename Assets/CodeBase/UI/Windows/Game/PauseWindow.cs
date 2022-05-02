using System;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows.Game
{
    public class PauseWindow : BaseWindow
    {
        [SerializeField] private Button _mainMenuButton;
        
        public event Action MenuButtonClicked;

        protected override void SubscribeUpdates()
        {
            base.SubscribeUpdates();
            _mainMenuButton.onClick.AddListener(OnMenuButtonClicked);
        }

        private void OnMenuButtonClicked()
        {
            MenuButtonClicked?.Invoke();
            gameObject.SetActive(false);
        }

        protected override void Cleanup()
        {
            base.Cleanup();
            _mainMenuButton.onClick.RemoveListener(OnMenuButtonClicked);
        }
    }
}
