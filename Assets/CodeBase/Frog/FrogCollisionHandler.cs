using System;
using CodeBase.Audio;
using UnityEngine;
using Zenject;

namespace CodeBase.Frog
{
    public class FrogCollisionHandler : IInitializable, IDisposable
    {
        private FrogPlayer _frogPlayer;
        private AudioPlayer _audioPlayer;
        private ParticleEmmiter _particleEmmiter;

        [Inject]
        private void Construct(FrogPlayer frogPlayer, AudioPlayer audioPlayer, ParticleEmmiter particleEmmiter)
        {
            _frogPlayer = frogPlayer;
            _audioPlayer = audioPlayer;
            _particleEmmiter = particleEmmiter;
        }

        public void Initialize()
        {
            _frogPlayer.Died += OnDied;
            _frogPlayer.ObstacleCollided += OnObstacleCollided;
        }
        
        private void OnDied() =>
            _particleEmmiter.Play(Particle.Death, _frogPlayer.transform.position);

        private void OnObstacleCollided(Vector2 collisionPoint)
        {
            _particleEmmiter.Play(Particle.Collision, collisionPoint);
            _particleEmmiter.Play(Particle.LightRing, collisionPoint);
            _audioPlayer.Collision();
        }

        public void Dispose()
        {
            _frogPlayer.Died -= OnDied;
            _frogPlayer.ObstacleCollided -= OnObstacleCollided;
        }
    }
}