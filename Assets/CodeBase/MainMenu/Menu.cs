using System;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Tasks;
using CodeBase.UI.Windows;
using CodeBase.UI.Windows.Menu;
using Zenject;

namespace CodeBase.MainMenu
{
    public class Menu : IInitializable, IDisposable
    {
        private const string GameSceneName = "Game";
        private readonly CrossfadeWindow _crossfadeWindow;
        private readonly MenuWindow _menuWindow;
        private readonly Shop _shop;
        private readonly SettingsWindow _settingsWindow;
        private readonly TaskCreator _taskCreator;
        private readonly SceneLoader _sceneLoader;
        private readonly PersistentProgressService _progressService;
        private readonly SaveLoadService _saveLoadService;
        private readonly ISavedProgressReader[] _progressReaders;
        private readonly ISavedProgress[] _progressWriters;

        public Menu(
            CrossfadeWindow crossfadeWindow, 
            MenuWindow menuWindow, 
            Shop shop, 
            SettingsWindow settingsWindow, 
            TaskCreator taskCreator,
            SceneLoader sceneLoader, 
            PersistentProgressService progressService ,
            SaveLoadService saveLoadService,
            ISavedProgressReader[] progressReaders,
            ISavedProgress[] progressWriters
            )
        {
            _crossfadeWindow = crossfadeWindow;
            _menuWindow = menuWindow;
            _shop = shop;
            _settingsWindow = settingsWindow;
            _taskCreator = taskCreator;
            _sceneLoader = sceneLoader;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
            _progressReaders = progressReaders;
            _progressWriters = progressWriters;
        }
        
        public void Initialize()
        {
            Subscribe();
            InformProgressReaders();

            //_menuWindow.Initialize();
            _menuWindow.Open();
            _shop.Initialize();
            //_settingsWindow.Initialize();
            _taskCreator.Initialize();

            _crossfadeWindow.Close();
        }

        private void Subscribe()
        {
            _menuWindow.PlayClicked += OnPlayClicked;
            _menuWindow.ShopClicked += OnShopClicked;
            _menuWindow.SettingsClicked += OnSettingsClicked;
            _shop.ShopWindowClosed += () => _menuWindow.Open();
            _settingsWindow.CloseButtonClicked += OnSettingsClosed;
        }
        
        private void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressReader in _progressReaders)
                progressReader.LoadProgress(_progressService.Progress);
        }

        private void OnPlayClicked() => 
            _crossfadeWindow.Open(() => _sceneLoader.Load(GameSceneName));

        private void OnShopClicked()
        {
            //_menuWindow.Close();
            _shop.OpenWindow();
        }

        private void OnSettingsClicked()
        {
            //_menuWindow.Close();
            _settingsWindow.Open();
        }

        private void OnSettingsClosed()
        {
            //_settingsWindow.CloseButtonClicked();
            _menuWindow.Open();
        }

        public void Dispose()
        {
            SaveProgress();
            Unsubscribe();

            _shop.Disable();
            //_settingsWindow.Cleanup();

            DG.Tweening.DOTween.KillAll();
        }

        private void SaveProgress() => 
            _saveLoadService.SaveProgress(_progressWriters);

        private void Unsubscribe()
        {
            _menuWindow.PlayClicked -= OnPlayClicked;
            _menuWindow.ShopClicked -= OnShopClicked;
            _menuWindow.SettingsClicked -= OnSettingsClicked;
            _settingsWindow.CloseButtonClicked -= OnSettingsClosed;
        }
    }
}