using System;
using CodeBase.Frog;
using UnityEngine;

namespace CodeBase.Coins
{
    public class Coin : MonoBehaviour
    {
        private const int CoinReward = 1;
        public int Reward => CoinReward;

        public event Action<Coin> CoinPicked;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out FrogPlayer frogPlayer))
            {
                CoinPicked?.Invoke(this);
                SetActiveState(false);
            }
        }

        public void SetActiveState(bool state) => 
            gameObject.SetActive(state);
    }
}
