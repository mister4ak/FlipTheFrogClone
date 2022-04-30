using System;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.MainMenu.PlayerShop;
using CodeBase.Tasks;
using CodeBase.UI.Windows;
using Zenject;

namespace CodeBase.MainMenu
{
    public class Menu : IInitializable, IDisposable
    {
        private const string _gameSceneName = "Game";
        private readonly CrossfadeWindow _crossfadeWindow;
        private readonly MenuWindow _menuWindow;
        private readonly Shop _shop;
        private readonly MenuSettings _menuSettings;
        private readonly PersistentProgressService _progressService;
        private readonly ISavedProgressReader[] _progressReaders;
        private readonly SceneLoader _sceneLoader;
        private readonly TaskCreator _taskCreator;
        private readonly ISavedProgress[] _progressWriters;
        private readonly SaveLoadService _saveLoadService;

        public Menu(CrossfadeWindow crossfadeWindow, SceneLoader sceneLoader, TaskCreator taskCreator,
            MenuWindow menuWindow, Shop shop, MenuSettings menuSettings, SaveLoadService saveLoadService,
            PersistentProgressService progressService ,ISavedProgressReader[] progressReaders, ISavedProgress[] progressWriters)
        {
            _saveLoadService = saveLoadService;
            _progressWriters = progressWriters;
            _taskCreator = taskCreator;
            _sceneLoader = sceneLoader;
            _progressService = progressService;
            _progressReaders = progressReaders;
            _menuSettings = menuSettings;
            _shop = shop;
            _menuWindow = menuWindow;
            _crossfadeWindow = crossfadeWindow;
        }


        public void Initialize()
        {
            Subscribe();
            InformProgressReaders();

            _menuWindow.Initialize();
            _shop.Initialize();
            _menuSettings.Initialize();
            _taskCreator.Initialize();

            _crossfadeWindow.Close();
        }

        private void Subscribe()
        {
            _menuWindow.PlayClicked += OnPlayClicked;
            _menuWindow.ShopClicked += OnShopClicked;
            _menuWindow.SettingsClicked += OnSettingsClicked;
            _shop.ShopWindowClosed += () => _menuWindow.Open();
            _menuSettings.Closed += OnMenuSettingsCloseClicked;
        }
        
        private void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressReader in _progressReaders)
                progressReader.LoadProgress(_progressService.Progress);
        }

        private void OnPlayClicked() => 
            _crossfadeWindow.Open(() => _sceneLoader.Load(_gameSceneName));

        private void OnShopClicked()
        {
            _menuWindow.Close();
            _shop.OpenWindow();
        }

        private void OnSettingsClicked()
        {
            _menuWindow.Close();
            _menuSettings.Open();
        }

        private void OnMenuSettingsCloseClicked()
        {
            _menuSettings.Close();
            _menuWindow.Open();
        }

        public void Dispose()
        {
            SaveProgress();
            Unsubscribe();

            _shop.Disable();
            _menuSettings.Disable();

            DG.Tweening.DOTween.KillAll();
        }

        private void SaveProgress() => 
            _saveLoadService.SaveProgress(_progressWriters);

        private void Unsubscribe()
        {
            _menuWindow.PlayClicked -= OnPlayClicked;
            _menuWindow.ShopClicked -= OnShopClicked;
            _menuWindow.SettingsClicked -= OnSettingsClicked;
            _menuSettings.Closed -= OnMenuSettingsCloseClicked;
        }
    }
}