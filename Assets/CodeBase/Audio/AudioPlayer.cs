using UnityEngine;

namespace CodeBase.Audio
{
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource _jumpSound;
        [SerializeField] private AudioSource _collisionSound;
        [SerializeField] private AudioSource _backgroundMusic;
        private bool _isAudioPaused;

        public void Initialize(bool isAudioEnabled)
        {
            _isAudioPaused = isAudioEnabled;
            AudioListener.pause = _isAudioPaused;
            //PlayBackgroundMusic();
        }

        private void PlayBackgroundMusic() => 
            _backgroundMusic.Play();

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
