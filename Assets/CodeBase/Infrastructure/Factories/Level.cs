using System.Linq;
using CodeBase.Coins;
using CodeBase.Colliders;
using CodeBase.Data;
using CodeBase.Infrastructure.Services;
using CodeBase.Obstacles;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Factories
{
    public class Level
    {
        private const string LevelsPath = "Levels/";
        private readonly int _levelID;
        private GameObject _currentLevel;
        //private StaticDataService _staticDataService;

        public Level(int levelID)
        {
            _levelID = levelID;
        }

        public void LoadLevel() => 
            InstantiateLevel();

        public void Destroy()
        {
            if (_currentLevel != null)
                Object.Destroy(_currentLevel);
        }

        private void InstantiateLevel()
        {
            GameObject levelPrefab = Resources.Load<GameObject>($"{LevelsPath}{_levelID}");
            //GameObject levelPrefab = Resources.Load<GameObject>($"{LevelsPath}{_staticDataService.LevelsData().levels[_levelID - 1]}");
            _currentLevel = Object.Instantiate(levelPrefab);
        }

        public LevelData GetData() =>
            new LevelData
            {
                coins = _currentLevel.GetComponentsInChildren<Coin>(),
                finishLine = _currentLevel.GetComponentInChildren<FinishLine>(),
                movingObstacles = _currentLevel.GetComponentsInChildren<ObstacleMover>().ToList()
            };

        public class Factory : PlaceholderFactory<int, Level>
        {
        }
    }
}