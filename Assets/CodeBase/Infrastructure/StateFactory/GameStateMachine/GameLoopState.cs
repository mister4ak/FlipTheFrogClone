using System;
using CodeBase.Colliders;
using CodeBase.Data;
using CodeBase.Frog;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Input;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.UI;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.StateFactory.GameStateMachine
{
    public class GameLoopState : IState, IInitializable, ISavedProgress
    {
        private const string MainMenuScene = "MainMenu";
        private const int LevelsRequiredForTraining = 3;
        
        private readonly FrogPlayer _frogPlayer;
        private readonly LevelUI _levelUI;
        private readonly LevelCreator _levelCreator;
        private readonly TimeService _timeService;
        private readonly SceneLoader _sceneLoader;
        private readonly IInputService _inputService;
        private FinishLine _finishLine;
        private PlayerData _playerData;
        private bool _isPaused;
        public event Action GameOver;
        public event Action LevelCompleted;
        
        public GameLoopState(
            FrogPlayer frogPlayer, 
            LevelUI levelUI, 
            LevelCreator levelCreator,
            TimeService timeService,
            SceneLoader sceneLoader,
            IInputService inputService
        )
        {
            _inputService = inputService;
            _sceneLoader = sceneLoader;
            _timeService = timeService;
            _levelCreator = levelCreator;
            _levelUI = levelUI;
            _frogPlayer = frogPlayer;
        }

        public void Initialize()
        {
            _levelCreator.LevelCreated += OnLevelCreated;
            _frogPlayer.Died += FrogDied;
            _levelUI.GamePaused += OnPausedGame;
            _levelUI.GameResumed += OnContinueGame;
            _levelUI.MenuClicked += LoadMenu;
        }

        private void OnContinueGame()
        {
            _isPaused = false;
            _timeService.Resume();
        }

        private void OnPausedGame()
        {
            _isPaused = true;
            _timeService.Stop();
        }

        public void OnEnter()
        {
            _levelUI.StartLevel();
            CheckTutorialCompleted();
            
            _inputService.Clicked += OnClicked;
            _inputService.Hold += OnHold;
            _inputService.Released += OnReleased;
        }

        private void CheckTutorialCompleted()
        {
            if (_playerData.isTutorialCompleted == false && _playerData.currentLevelIndex < LevelsRequiredForTraining)
                _levelUI.StartTutorial();
            else
                _playerData.isTutorialCompleted = true;
        }

        private void OnClicked(Vector2 position)
        {
            if (_isPaused)
                return;
            
            _frogPlayer.SetStartPosition(position);
            _timeService.SlowDown();
        }

        private void OnHold(Vector2 position)
        {
            if (_isPaused)
                return;
            
            _frogPlayer.SetHoldPosition(position);
        }

        private void OnReleased(Vector2 position)
        {
            if (_isPaused)
                return;
            
            _frogPlayer.SetReleasedPosition(position);
            _timeService.Resume();
        }

        public void OnExit()
        {
            _frogPlayer.LevelEnded();
            _inputService.Clicked -= OnClicked;
            _inputService.Hold -= OnHold;
            _inputService.Released -= OnReleased;
        }

        private void FrogDied()
        {
            _frogPlayer.gameObject.SetActive(false);
            StopTimeDilation();
            GameOver?.Invoke();
        }

        private void StopTimeDilation()
        {
            if (_timeService.IsTimeSlowDown)
                _timeService.Resume();
        }

        private void OnLevelCreated(LevelData levelData)
        {
            _finishLine = levelData.finishLine;
            _finishLine.OnFinish += FrogFinished;
        }

        private void LoadMenu()
        {
            _timeService.Resume();
            _sceneLoader.Load(MainMenuScene);
        }

        private void FrogFinished()
        {
            LevelCompleted?.Invoke();
            _finishLine.OnFinish -= FrogFinished;
            _levelUI.FinishLevel();
        }

        public void LoadProgress(PlayerProgress progress) => 
            _playerData = progress.PlayerData;

        public void UpdateProgress(PlayerProgress progress) => 
            progress.PlayerData.isTutorialCompleted = _playerData.isTutorialCompleted;
    }
}