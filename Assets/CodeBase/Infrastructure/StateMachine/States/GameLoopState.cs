using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Colliders;
using CodeBase.Data;
using CodeBase.Frog;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Input;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace CodeBase.Infrastructure.StateMachine.States
{
    public class GameLoopState : IState, IInitializable, ISavedProgress
    {
        private const string MainMenuScene = "MainMenu";
        private const string TutorialLayerName = "TutorialWindow";
        private const int LevelsRequiredForTraining = 3;

        private readonly FrogPlayer _frogPlayer;
        private readonly LevelUI _levelUI;
        private readonly LevelCreator _levelCreator;
        private readonly TimeService _timeService;
        private readonly SceneLoader _sceneLoader;
        private readonly IInputService _inputService;
        
        private FinishLine _finishLine;
        private PlayerData _playerData;
        private PointerEventData _eventData;
        private List<RaycastResult> _raycastResults;
        
        private bool _isPaused;
        private int _tutorialLayer;

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
            _frogPlayer = frogPlayer;
            _levelUI = levelUI;
            _levelCreator = levelCreator;
            _timeService = timeService;
            _sceneLoader = sceneLoader;
            _inputService = inputService;
        }

        public void Initialize()
        {
            _levelCreator.LevelCreated += OnLevelCreated;
            _frogPlayer.Died += FrogDied;
            _levelUI.GamePaused += OnPausedGame;
            _levelUI.GameResumed += OnContinueGame;
            _levelUI.MenuClicked += LoadMenu;
            
            InitEventSystemForUIHit();
        }

        private void InitEventSystemForUIHit()
        {
            _eventData = new PointerEventData(EventSystem.current);
            _raycastResults = new List<RaycastResult>();
            _tutorialLayer = LayerMask.NameToLayer(TutorialLayerName);
        }

        public void OnEnter()
        {
            _levelUI.StartLevel();
            CheckTutorialCompleted();
            SubscribeOnInput();
        }

        private void CheckTutorialCompleted()
        {
            if (_playerData.isTutorialCompleted == false && _playerData.currentLevelIndex < LevelsRequiredForTraining)
                _levelUI.StartTutorial();
            else
                _playerData.isTutorialCompleted = true;
        }

        private void SubscribeOnInput()
        {
            _inputService.Clicked += OnClicked;
            _inputService.Hold += OnHold;
            _inputService.Released += OnReleased;
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

            if (HitUI(position))
                return;

            _frogPlayer.SetReleasedPosition(position);
            _timeService.Resume();
        }

        private bool HitUI(Vector2 position)
        {
            _eventData.position = position;
            EventSystem.current.RaycastAll(_eventData, _raycastResults);
            
            if (HitNotTutorialLayer())
                return false;

            return _raycastResults.Count > 0;
        }

        private bool HitNotTutorialLayer() => 
            _raycastResults.Any(raycastResult => _tutorialLayer == 1 << raycastResult.gameObject.layer);

        public void OnExit()
        {
            _frogPlayer.DisableArrow();
            UnsubscribeOnInput();
        }

        private void UnsubscribeOnInput()
        {
            _inputService.Clicked -= OnClicked;
            _inputService.Hold -= OnHold;
            _inputService.Released -= OnReleased;
        }

        private void OnLevelCreated(LevelData levelData)
        {
            _finishLine = levelData.finishLine;
            _finishLine.OnFinish += FrogFinished;
        }

        private void FrogFinished()
        {
            LevelCompleted?.Invoke();
            _finishLine.OnFinish -= FrogFinished;
            _levelUI.FinishLevel();
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

        private void OnPausedGame()
        {
            _isPaused = true;
            _timeService.Stop();
        }

        private void OnContinueGame()
        {
            _frogPlayer.DisableArrow();
            _isPaused = false;
            _timeService.Resume();
        }

        private void LoadMenu()
        {
            _timeService.Resume();
            _sceneLoader.Load(MainMenuScene);
        }

        public void LoadProgress(PlayerProgress progress) => 
            _playerData = progress.PlayerData;

        public void UpdateProgress(PlayerProgress progress) => 
            progress.PlayerData.isTutorialCompleted = _playerData.isTutorialCompleted;
    }
}