using System;

namespace CodeBase.Data
{
    [Serializable]
    public class PlayerData
    {
        public int coins;
        public int currentLevelIndex;
        public int levelsCount;
        public bool isTutorialCompleted;

        public Action CoinsChanged;

        public void AddCoins(int amount)
        {
            if (amount <= 0)
                throw new ArgumentException();
            coins += amount;
            CoinsChanged?.Invoke();
        }
        public void ReduceCoins(int amount)
        {
            if (amount <= 0)
                throw new ArgumentException();
            coins -= amount;
            CoinsChanged?.Invoke();
        }
    }
}