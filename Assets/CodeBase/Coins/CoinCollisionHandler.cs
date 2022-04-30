using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using Zenject;

namespace CodeBase.Coins
{
    public class CoinCollisionHandler
    {
        private ParticleEmmiter _particleEmmiter;
        private PlayerProgress _progress;
        private Coin[] _coins;

        [Inject]
        private void Construct(ParticleEmmiter particleEmmiter, PersistentProgressService progressService)
        {
            _particleEmmiter = particleEmmiter;
            _progress = progressService.Progress;
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