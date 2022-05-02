using System;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.StateMachine.States;
using Zenject;

namespace CodeBase.Infrastructure.StateMachine
{
    public class Game : IInitializable, IDisposable
    {
        private BaseStateMachine _baseStateMachine;
        private readonly LoadLevelState _loadLevelState;
        private readonly GameLoopState _gameLoopState;
        private readonly GameOverState _gameOverState;
        private readonly EndLevelState _endLevelState;
        private readonly ISavedProgressReader[] _progressReaders;
        private readonly ISavedProgress[] _progressWriters;
        private readonly PersistentProgressService _progressService;
        private readonly SaveLoadService _saveLoadService;

        public Game(
            LoadLevelState loadLevelState,
            GameLoopState gameLoopState,
            GameOverState gameOverState,
            EndLevelState endLevelState,
            ISavedProgressReader[] progressReaders,
            ISavedProgress[] progressWriters,
            PersistentProgressService progressService,
            SaveLoadService saveLoadService
        )
        {
            _loadLevelState = loadLevelState;
            _gameLoopState = gameLoopState;
            _gameOverState = gameOverState;
            _endLevelState = endLevelState;
            _progressReaders = progressReaders;
            _progressWriters = progressWriters;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
        }

        public void Initialize()
        {
            _baseStateMachine = new BaseStateMachine();
            _loadLevelState.StateLoaded += () => ChangeState(_gameLoopState);

            _gameLoopState.GameOver += () => ChangeState(_gameOverState);
            _gameLoopState.LevelCompleted += () => ChangeState(_endLevelState);

            _gameOverState.LevelRestarted += RestartLevel;
            _endLevelState.LevelEnded += LoadLevelState;

            InformProgressReaders();
            ChangeState(_loadLevelState);
        }

        private void ChangeState(IState state) => 
            _baseStateMachine.SetState(state);

        private void RestartLevel()
        {
            SaveProgress();
            ChangeState(_gameLoopState);
        }
        
        private void LoadLevelState()
        {
            SaveProgress();
            InformProgressReaders();
            ChangeState(_loadLevelState);
        }

        private void SaveProgress() => 
            _saveLoadService.SaveProgress(_progressWriters);

        private void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressReader in _progressReaders)
                progressReader.LoadProgress(_progressService.Progress);
        }

        public void Dispose()
        {
            SaveProgress();
            DG.Tweening.DOTween.KillAll();
        }
    }
}