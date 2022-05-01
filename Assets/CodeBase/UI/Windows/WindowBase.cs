using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows
{
    public abstract class WindowBase : MonoBehaviour
    {
        // protected IPersistentProgressService ProgressService;
        // protected PlayerProgress Progress => ProgressService.Progress;
        
        [SerializeField] protected Button CloseButton;

        // public void Construct(IPersistentProgressService progressService) => 
        //     ProgressService = progressService;

        public void Open()
        {
            gameObject.SetActive(true);
            Initialize();
            SubscribeUpdates();
        }

        // public virtual void Close()
        // {
        //     gameObject.SetActive(false);
        // }

        private void Start()
        {
            Initialize();
            SubscribeUpdates();
        }

        private void OnDisable()
        {
            Cleanup();
        }

        protected virtual void Initialize(){}
        protected virtual void SubscribeUpdates(){}
        protected virtual void Cleanup(){}
    }
}
