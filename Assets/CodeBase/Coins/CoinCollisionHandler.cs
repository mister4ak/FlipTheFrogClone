using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using Zenject;

namespace CodeBase.Coins
{
    public class CoinCollisionHandler
    {
        private ParticleEmmiter _particleEmmiter;
        private Coin[] _coins;
        private PlayerProgress _progress;

        [Inject]
        private void Construct(ParticleEmmiter particleEmmiter, PersistentProgressService progressService)
        {
            _progress = progressService.Progress;
            _particleEmmiter = particleEmmiter;
        }

        public void Initialize(Coin[] coins)
        {
            _coins = coins;
            foreach (Coin coin in _coins)
                coin.CoinPicked += OnCoinPicked;
        }

        private void OnCoinPicked(Coin coin)
        {
            PlayParticleEffects(coin);
            UpdateCoinsAmount(coin);
            Vibrator.Vibrate(50);
        }

        private void PlayParticleEffects(Coin coin) => 
            _particleEmmiter.Play(Particle.Coin, coin.transform.position);

        private void UpdateCoinsAmount(Coin coin) => 
            _progress.PlayerData.AddCoins(coin.Reward);

        public void Cleanup()
        {
            foreach (Coin coin in _coins)
                coin.CoinPicked -= OnCoinPicked;
        }
    }
}