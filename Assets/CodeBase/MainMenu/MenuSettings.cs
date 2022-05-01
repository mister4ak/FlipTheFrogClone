using System;
using CodeBase.Audio;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.MainMenu
{
    public class MenuSettings : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private Button _soundButton;
        [SerializeField] private Button _vibrationButton;

        [SerializeField] private Sprite _volumeOnImage;
        [SerializeField] private Sprite _volumeOffImage;
        [SerializeField] private Sprite _vibrationOnImage;
        [SerializeField] private Sprite _vibrationOffImage;
    
        [SerializeField] private Button _settingsCloseButton;

        private Image _soundImage;
        private Image _vibrationImage;

        private AudioPlayer _audioPlayer;
        private PlayerSettings _settingsData;

        public event Action Closed;

        [Inject]
        private void Construct(AudioPlayer audioPlayer)
        {
            _audioPlayer = audioPlayer;
        }
        
        public void Initialize()
        {
            Subscribe();
            
            _soundImage = _soundButton.GetComponent<Image>();
            _vibrationImage = _vibrationButton.GetComponent<Image>();
            ChangeSoundImage(_settingsData.isAudioPaused);
            ChangeVibrationImage(_settingsData.isVibrationEnabled);
        }

        private void Subscribe()
        {
            _soundButton.onClick.AddListener(OnSoundButtonClick);
            _vibrationButton.onClick.AddListener(OnVibrationButtonClick);
        }

        public void Open()
        {
            gameObject.SetActive(true);
            _settingsCloseButton.onClick.AddListener(CloseButtonClicked);
        }

        public void CloseButtonClicked()
        {
            Closed?.Invoke();
            gameObject.SetActive(false);
        }

        private void OnSoundButtonClick()
        {
            _settingsData.isAudioPaused = !_settingsData.isAudioPaused;
            ChangeSoundImage(_settingsData.isAudioPaused);
            _audioPlayer.ChangeAudioState(_settingsData.isAudioPaused);
        }

        private void OnVibrationButtonClick()
        {
            _settingsData.isVibrationEnabled = !_settingsData.isVibrationEnabled;
            ChangeVibrationImage(_settingsData.isVibrationEnabled);
            Vibrator.ChangeVibratorState(_settingsData.isVibrationEnabled);
        }

        private void ChangeSoundImage(bool isAudioPaused) => 
            _soundImage.sprite = isAudioPaused ? _volumeOffImage : _volumeOnImage;

        private void ChangeVibrationImage(bool isVibrationEnabled) => 
            _vibrationImage.sprite = isVibrationEnabled ? _vibrationOnImage : _vibrationOffImage;

        public void Cleanup()
        {
            _soundButton.onClick.RemoveListener(OnSoundButtonClick);
            _vibrationButton.onClick.RemoveListener(OnVibrationButtonClick);
        }

        public void LoadProgress(PlayerProgress progress) => 
            _settingsData = progress.PlayerSettings;

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.PlayerSettings.isAudioPaused = _settingsData.isAudioPaused;
            progress.PlayerSettings.isVibrationEnabled = _settingsData.isVibrationEnabled;
        }
    }
}
