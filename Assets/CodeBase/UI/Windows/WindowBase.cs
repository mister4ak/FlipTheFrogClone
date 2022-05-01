using System;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows
{
    public abstract class WindowBase : MonoBehaviour
    {
        [SerializeField] protected Button _closeButton;

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
        protected virtual void SubscribeUpdates(){}
        protected virtual void Cleanup(){}
    }
}
