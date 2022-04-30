using System;
using UnityEngine;

namespace CodeBase
{
    public enum Particle{
        Death,
        Collision,
        LightRing,
        Jump,
        Coin
    }

    public class ParticleEmmiter : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _deathParticlePrefab;
        [SerializeField] private ParticleSystem _collisionParticlePrefab;
        [SerializeField] private ParticleSystem _lightRingParticlePrefab;
        [SerializeField] private ParticleSystem _jumpParticlePrefab;
        [SerializeField] private ParticleSystem _coinParticlePrefab;

        private ParticleSystem _particleSystem;

        public void Play(Particle particle, Vector2 particlePosition)
        {
            switch (particle)
            {
                case Particle.Death:
                    _particleSystem = Instantiate(_deathParticlePrefab);
                    break;
                case Particle.Collision:
                    _particleSystem = Instantiate(_collisionParticlePrefab);
                    break;
                case Particle.LightRing:
                    _particleSystem = Instantiate(_lightRingParticlePrefab);
                    break;
                case Particle.Jump:
                    _particleSystem = Instantiate(_jumpParticlePrefab);
                    break;
                case Particle.Coin:
                    _particleSystem = Instantiate(_coinParticlePrefab);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(particle), particle, null);
            }

            _particleSystem.transform.position = particlePosition;
            _particleSystem.Play();
            Destroy(_particleSystem.gameObject, _particleSystem.main.duration);
        }
    }
}