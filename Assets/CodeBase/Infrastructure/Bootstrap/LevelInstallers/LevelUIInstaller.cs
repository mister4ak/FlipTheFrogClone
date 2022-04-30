using CodeBase.UI;
using CodeBase.UI.Windows;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Bootstrap.LevelInstallers
{
    public class LevelUIInstaller : MonoInstaller
    {
        [SerializeField] private GameHud _gameHudPrefab;
        [SerializeField] private PauseWindow _pauseWindowPrefab;
        [SerializeField] private GameOverWindow _gameOverWindow;
        [SerializeField] private EndLevelWindow _endLevelWindow;
        [SerializeField] private TutorialWindow _tutorialWindow;
        
        private GameHud _gameHud;

        public override void InstallBindings()
        {
            BindGameHud();
            BindPauseWindow();
            BindGameOverWindow();
            BindEndLevelWindow();
            BindTutorialWindow();
            BindLevelUIScript();
        }

        private void BindGameHud()
        {
            _gameHud = Container
                .InstantiatePrefabForComponent<GameHud>(_gameHudPrefab);
            
            Container
                .BindInterfacesAndSelfTo<GameHud>()
                .FromInstance(_gameHud)
                .AsSingle();
        }

        private void BindPauseWindow()
        {
            PauseWindow pauseWindow = Container
                .InstantiatePrefabForComponent<PauseWindow>(_pauseWindowPrefab, _gameHud.transform);
            
            Container
                .BindInterfacesAndSelfTo<PauseWindow>()
                .FromInstance(pauseWindow)
                .AsSingle();
        }

        private void BindGameOverWindow()
        {
            GameOverWindow gameOverWindow = Container
                .InstantiatePrefabForComponent<GameOverWindow>(_gameOverWindow, _gameHud.transform);
            
            Container
                .BindInterfacesAndSelfTo<GameOverWindow>()
                .FromInstance(gameOverWindow)
                .AsSingle();
        }

        private void BindEndLevelWindow()
        {
            EndLevelWindow endLevelWindow = Container
                .InstantiatePrefabForComponent<EndLevelWindow>(_endLevelWindow, _gameHud.transform);
            
            Container
                .BindInterfacesAndSelfTo<EndLevelWindow>()
                .FromInstance(endLevelWindow)
                .AsSingle();
        }

        private void BindTutorialWindow()
        {
            Container
                .BindFactory<TutorialWindow, TutorialWindow.Factory>()
                .FromComponentInNewPrefab(_tutorialWindow)
                .UnderTransform(_gameHud.transform)
                .AsSingle();
        }

        private void BindLevelUIScript()
        {
            Container
                .BindInterfacesAndSelfTo<LevelUI>()
                .AsSingle();
        }
    }
}