using CodeBase.Infrastructure.Services;
using CodeBase.MainMenu;
using CodeBase.Tasks;
using CodeBase.UI.Menu;
using CodeBase.UI.Menu.Shop;
using CodeBase.UI.Windows.Menu;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Installers
{
    public class MenuInstaller : MonoInstaller
    {
        [SerializeField] private Transform _uiRoot;

        [SerializeField] private MenuWindow _menuWindowPrefab;
        [SerializeField] private ShopWindow _shopWindowPrefab;
        [SerializeField] private SettingsWindow _settingsWindowPrefab;
        [SerializeField] private TaskUI _taskUIPrefab;

        private MenuWindow _menuWindow;

        public override void InstallBindings()
        {
            BindAdsService(); 
            BindMenuWindow();
            BindShop();
            BindSettings();
            BindTasks();
            BindMenu();
        }

        private void BindAdsService() => 
            Container.BindInterfacesAndSelfTo<AdsService>().AsSingle();

        private void BindMenuWindow()
        {
            _menuWindow = Container
                .InstantiatePrefabForComponent<MenuWindow>(_menuWindowPrefab, _uiRoot);

            Container.BindInterfacesAndSelfTo<MenuWindow>().FromInstance(_menuWindow).AsSingle();
        }

        private void BindShop()
        {
            ShopWindow shopWindow = Container
                .InstantiatePrefabForComponent<ShopWindow>(_shopWindowPrefab, _uiRoot);

            Container.BindInterfacesAndSelfTo<ShopWindow>().FromInstance(shopWindow).AsSingle();
            
            Container.BindInterfacesAndSelfTo<ScrollUI>()
                .FromInstance(shopWindow.GetComponentInChildren<ScrollUI>())
                .AsSingle();
            
            Container.Bind<RewardedAdItem>()
                .FromInstance(shopWindow.GetComponentInChildren<RewardedAdItem>())
                .AsSingle();

            Container.BindInterfacesAndSelfTo<Shop>().AsSingle();
        }

        private void BindSettings()
        {
            SettingsWindow settingsWindow = Container
                .InstantiatePrefabForComponent<SettingsWindow>(_settingsWindowPrefab, _uiRoot);

            Container.BindInterfacesAndSelfTo<SettingsWindow>().FromInstance(settingsWindow).AsSingle();
        }

        private void BindTasks()
        {
            TaskUI taskUI = Container
                .InstantiatePrefabForComponent<TaskUI>(_taskUIPrefab, _menuWindow.transform);

            Container.BindInterfacesAndSelfTo<TaskCreator>().AsSingle();
            Container.Bind<TaskUI>().FromInstance(taskUI).AsSingle();
        }

        private void BindMenu() => 
            Container.BindInterfacesAndSelfTo<Menu>().AsSingle().NonLazy();
    }
}