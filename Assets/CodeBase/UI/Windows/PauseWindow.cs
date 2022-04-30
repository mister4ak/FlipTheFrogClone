using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Windows
{
    public class PauseWindow : MonoBehaviour
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _mainMenuButton;

        public event Action PlayButtonClicked;
        public event Action MenuButtonClicked;

        public void Open()
        {
            gameObject.SetActive(true);
            _playButton.onClick.AddListener(() => PlayButtonClicked?.Invoke());
            _mainMenuButton.onClick.AddListener(() => MenuButtonClicked?.Invoke());
        }

        public void Close() => 
            gameObject.SetActive(false);
    }
}
