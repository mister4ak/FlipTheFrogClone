using System;
using CodeBase.UI.Windows;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace CodeBase.UI
{
    public class LevelUI : IInitializable
    {
        private CrossfadeWindow _crossfadeWindow;
        private PauseWindow _pauseWindow;
        private GameHud _gameHud;
        private GameOverWindow _gameOverWindow;
        private EndLevelWindow _endLevelWindow;
        private TutorialWindow _tutorialWindow;
        private TutorialWindow.Factory _tutorialFactory;

        public event Action LevelRestart;
        public event Action NextLevel;
        public event Action GamePaused;
        public event Action GameResumed;
        public event Action MenuClicked;
        

        [Inject]
        private void Construct(
            CrossfadeWindow crossfadeWindow,
            GameHud gameHud,
            PauseWindow pauseWindow,
            GameOverWindow gameOverWindow,
            EndLevelWindow endLevelWindow, 
            TutorialWindow.Factory tutorialFactory
        )
        {
            _tutorialFactory = tutorialFactory;
            _crossfadeWindow = crossfadeWindow;
            _gameHud = gameHud;
            _pauseWindow = pauseWindow;
            _gameOverWindow = gameOverWindow;
            _endLevelWindow = endLevelWindow;
        }

        public void Initialize()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            _gameHud.PauseButtonClicked += OpenPauseScreen;
            _pauseWindow.PlayButtonClicked += ClosePauseWindow;
            _pauseWindow.MenuButtonClicked += LoadMenu;

            _gameOverWindow.RestartButtonClicked += CloseGameOverWindow;
        }

        public void StartLevel()
        {
            _crossfadeWindow.Close();
            _gameHud.ShowPauseButton();
        }

        public void FinishLevel() => 
            _gameHud.HidePauseButton();

        public void OpenGameOverWindow()
        {
            _gameOverWindow.Open();
        }

        private void CloseGameOverWindow()
        {
            _crossfadeWindow.Open();
            //_gameOverWindow.Close();
            LevelRestart?.Invoke();
        }

        private void OpenPauseScreen()
        {
            _pauseWindow.Open();
            _gameHud.HidePauseButton();
            GamePaused?.Invoke();
        }

        private void ClosePauseWindow()
        {
            _pauseWindow.Close();
            _gameHud.ShowPauseButton();
            GameResumed?.Invoke();
        }

        private void LoadMenu()
        {
            _crossfadeWindow.Open(() => MenuClicked?.Invoke());
            _pauseWindow.Close();
        }

        public void OpenEndLevelWindow()
        {
            _endLevelWindow.Open();
            _endLevelWindow.EndLevelScreenClicked += CloseEndLevelWindow;
        }

        private void CloseEndLevelWindow()
        {
            _endLevelWindow.EndLevelScreenClicked -= CloseEndLevelWindow;
            //_endLevelWindow.Close();
            _crossfadeWindow.Open(() => NextLevel?.Invoke());
        }

        public void StartTutorial()
        {
            _tutorialWindow =_tutorialFactory.Create();

            _tutorialWindow.Open();
            _tutorialWindow.StartTutorial();
        }
    }
}

