using System;
using CodeBase.UI.Windows;
using Zenject;

namespace CodeBase.UI
{
    public class LevelUI : IInitializable
    {
        private readonly CrossfadeWindow _crossfadeWindow;
        private readonly PauseWindow _pauseWindow;
        private readonly GameHud _gameHud;
        private readonly GameOverWindow _gameOverWindow;
        private readonly EndLevelWindow _endLevelWindow;
        private readonly TutorialWindow.Factory _tutorialFactory;
        private TutorialWindow _tutorialWindow;

        public event Action LevelRestart;
        public event Action NextLevel;
        public event Action GamePaused;
        public event Action GameResumed;
        public event Action MenuClicked;
        

        [Inject]
        public LevelUI(
            CrossfadeWindow crossfadeWindow,
            PauseWindow pauseWindow,
            GameHud gameHud,
            GameOverWindow gameOverWindow,
            EndLevelWindow endLevelWindow, 
            TutorialWindow.Factory tutorialFactory
        )
        {
            _crossfadeWindow = crossfadeWindow;
            _pauseWindow = pauseWindow;
            _gameHud = gameHud;
            _gameOverWindow = gameOverWindow;
            _endLevelWindow = endLevelWindow;
            _tutorialFactory = tutorialFactory;
        }

        public void Initialize() => 
            Subscribe();

        private void Subscribe()
        {
            _gameHud.PauseButtonClicked += OpenPauseScreen;
            _pauseWindow.CloseButtonClicked += ClosePauseWindow;
            _pauseWindow.MenuButtonClicked += LoadMenu;
            
            _gameOverWindow.CloseButtonClicked += CloseGameOverWindow;
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
            _gameHud.ShowPauseButton();
            GameResumed?.Invoke();
        }

        private void LoadMenu() => 
            _crossfadeWindow.Open(() => MenuClicked?.Invoke());

        public void OpenEndLevelWindow()
        {
            _endLevelWindow.Open();
            _endLevelWindow.CloseButtonClicked += CloseEndLevelWindow;
        }

        private void CloseEndLevelWindow()
        {
            _endLevelWindow.CloseButtonClicked -= CloseEndLevelWindow;
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

