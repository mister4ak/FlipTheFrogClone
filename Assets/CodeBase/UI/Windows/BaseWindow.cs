using System;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows
{
    public abstract class BaseWindow : MonoBehaviour
    {
        [SerializeField] protected Button _closeButton;
        protected const float MaxAlpha = 1f;
        protected const float MinAlpha = 0f;
        protected const float FadeDuration = 0.5f;
        public event Action CloseButtonClicked;

        public void Open()
        {
            gameObject.SetActive(true);
            Initialize();
            SubscribeUpdates();
        }

        private void OnDisable() => 
            Cleanup();

        protected virtual void Initialize(){}

        protected virtual void SubscribeUpdates() => 
            _closeButton.onClick.AddListener(OnCloseButtonClicked);

        private void OnCloseButtonClicked()
        {
            CloseButtonClicked?.Invoke();
            gameObject.SetActive(false);
        }

        protected virtual void Cleanup() => 
            _closeButton.onClick.RemoveListener(OnCloseButtonClicked);
    }
}
