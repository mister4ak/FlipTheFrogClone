using System;
using CodeBase.Coins;
using CodeBase.Data;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Obstacles;
using CodeBase.Tasks;
using UnityEngine;

namespace CodeBase.Infrastructure.StateMachine.States
{
    public class LoadLevelState : IState, ISavedProgressReader
    {
        private readonly CoinCollisionHandler _coinCollisionHandler;
        private readonly LevelCreator _levelCreator;
        private readonly TaskTracker _taskTracker;

        private PlayerData _playerData;
        private LevelData _levelData;
        private bool _isLevelLoaded;
        
        public event Action StateLoaded;

        public LoadLevelState(
            CoinCollisionHandler coinCollisionHandler, 
            LevelCreator levelCreator, 
            TaskTracker taskTracker
            )
        {
            _coinCollisionHandler = coinCollisionHandler;
            _levelCreator = levelCreator;
            _taskTracker = taskTracker;
        }
        
        public void OnEnter()
        {
            if (_isLevelLoaded)
                Cleanup();
            
            LoadLevel();
            InitializeMovingObstacles();
            _coinCollisionHandler.Initialize(_levelData.coins);
            _taskTracker.StartTaskTrackingIfHas();

            StateLoaded?.Invoke();
        }

        public void OnExit(){}

        private void LoadLevel()
        {
            if (_playerData.currentLevelIndex > _playerData.levelsCount)
                _playerData.currentLevelIndex = _playerData.levelsCount;
            
            _levelCreator.LoadCurrentLevel(_playerData.currentLevelIndex);
            _levelData = _levelCreator.GetData();
            _isLevelLoaded = true;
        }

        private void InitializeMovingObstacles()
        {
            foreach (ObstacleMover obstacleMover in _levelData.movingObstacles)
                obstacleMover.Initialize();
        }

        private void Cleanup()
        {
            DisableMovingObstacles();
            _coinCollisionHandler.Cleanup();
            _levelCreator.DestroyCurrentLevel();
        }

        private void DisableMovingObstacles()
        {
            foreach (ObstacleMover obstacleMover in _levelData.movingObstacles)
                obstacleMover.Disable();
        }

        public void LoadProgress(PlayerProgress progress) => 
            _playerData = progress.PlayerData;
    }
}