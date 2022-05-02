using System;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public class GameHud : MonoBehaviour
    {
        [SerializeField] private Button _pauseButton;
        public event Action PauseButtonClicked;
        
        private void Start() => 
            _pauseButton.onClick.AddListener(() => PauseButtonClicked?.Invoke());

        public void ShowPauseButton() => 
            _pauseButton.gameObject.SetActive(true);

        public void HidePauseButton() => 
            _pauseButton.gameObject.SetActive(false);
    }
}