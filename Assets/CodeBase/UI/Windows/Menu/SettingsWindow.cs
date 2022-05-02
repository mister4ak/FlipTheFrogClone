using CodeBase.Audio;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Windows.Menu
{
    public class SettingsWindow : BaseWindow, ISavedProgress
    {
        [SerializeField] private Button _soundButton;

        [SerializeField] private Sprite _volumeOnImage;
        [SerializeField] private Sprite _volumeOffImage;
        
        private Image _soundImage;

        private AudioPlayer _audioPlayer;
        private PlayerSettings _settingsData;

        [Inject]
        private void Construct(AudioPlayer audioPlayer)
        {
            _audioPlayer = audioPlayer;
        }

        protected override void Initialize()
        {
            _soundImage = _soundButton.GetComponent<Image>();
            ChangeSoundImage(_settingsData.isAudioPaused);
        }

        protected override void SubscribeUpdates()
        {
            base.SubscribeUpdates();
            _soundButton.onClick.AddListener(OnSoundButtonClick);
        }

        protected override void Cleanup()
        {
            base.Cleanup();
            _soundButton.onClick.RemoveListener(OnSoundButtonClick);
        }

        private void OnSoundButtonClick()
        {
            _settingsData.isAudioPaused = !_settingsData.isAudioPaused;
            ChangeSoundImage(_settingsData.isAudioPaused);
            _audioPlayer.ChangeAudioState(_settingsData.isAudioPaused);
        }

        private void ChangeSoundImage(bool isAudioPaused) => 
            _soundImage.sprite = isAudioPaused ? _volumeOffImage : _volumeOnImage;

        public void LoadProgress(PlayerProgress progress) => 
            _settingsData = progress.PlayerSettings;

        public void UpdateProgress(PlayerProgress progress) => 
            progress.PlayerSettings.isAudioPaused = _settingsData.isAudioPaused;
    }
}
