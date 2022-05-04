using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using Zenject;

namespace CodeBase.Audio
{
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource _jumpSound;
        [SerializeField] private AudioSource _collisionSound;
        [SerializeField] private AudioSource _backgroundMusic;
        private PersistentProgressService _progressService;
        private bool _isAudioPaused;

        [Inject]
        private void Construct(PersistentProgressService progressService) => 
            _progressService = progressService;

        private void Awake() => 
            DontDestroyOnLoad(gameObject);

        private void Start()
        {
            _isAudioPaused = _progressService.Progress.PlayerSettings.isAudioPaused;
            AudioListener.pause = _isAudioPaused;
        }

        public void Jump()
        {
            if (_isAudioPaused)
                return;
            
            _jumpSound.pitch = Random.Range(0.9f, 1.1f);
            _jumpSound.Play();
        }

        public void Collision()
        {
            if (_isAudioPaused)
                return;
            
            _collisionSound.pitch = Random.Range(0.9f, 1.1f);
            _collisionSound.Play();
        }

        public void ChangeAudioState(bool isAudioPaused)
        {
            _isAudioPaused = isAudioPaused;
            AudioListener.pause = _isAudioPaused;
        }
    }
}
