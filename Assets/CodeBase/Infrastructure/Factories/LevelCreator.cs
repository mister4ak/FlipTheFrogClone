using System;
using CodeBase.Data;

namespace CodeBase.Infrastructure.Factories
{
    public class LevelCreator
    {
        private Level.Factory _levelFactory;
        private Level _currentLevel;
        private LevelData _currentLevelData;

        public event Action<LevelData> LevelCreated;

        public LevelCreator(Level.Factory levelFactory)
        {
            _levelFactory = levelFactory;
        }

        public void LoadCurrentLevel(int levelId)
        {
            _currentLevel = _levelFactory.Create(levelId);
            _currentLevel.LoadLevel();
            _currentLevelData = _currentLevel.GetData();
            
            LevelCreated?.Invoke(_currentLevelData);
        }
        
        public void DestroyCurrentLevel() =>
            _currentLevel.Destroy();


        public LevelData GetData() => 
            _currentLevelData;
    }
}