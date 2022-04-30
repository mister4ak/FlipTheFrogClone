using CodeBase.Infrastructure.Services;
using CodeBase.MainMenu;
using CodeBase.MainMenu.PlayerShop;
using CodeBase.Tasks;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Bootstrap
{
    public class MenuInstaller : MonoInstaller
    {
        [SerializeField] private Transform _uiRoot;

        [SerializeField] private MenuWindow _menuWindowPrefab;
        [SerializeField] private ShopWindow _shopWindowPrefab;
        [SerializeField] private MenuSettings _menuSettingsPrefab;
        [SerializeField] private TaskUI _taskUIPrefab;

        private MenuWindow _menuWindow;

        public override void InstallBindings()
        {
            BindMenuWindow();
            BindAds(); 
            BindShop();
            BindSettings();
            BindTasks();
            BindMenu();
        }

        private void BindAds()
        {
            Container.BindInterfacesAndSelfTo<AdsService>().AsSingle();
        }

        private void BindMenuWindow()
        {
            _menuWindow = Container
                .InstantiatePrefabForComponent<MenuWindow>(_menuWindowPrefab, _uiRoot);

            Container
                .BindInterfacesAndSelfTo<MenuWindow>()
                .FromInstance(_menuWindow)
                .AsSingle();
        }

        private void BindShop()
        {
            ShopWindow shopWindow = Container
                .InstantiatePrefabForComponent<ShopWindow>(_shopWindowPrefab, _uiRoot);

            Container
                .BindInterfacesAndSelfTo<ShopWindow>()
                .FromInstance(shopWindow)
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<ScrollUI>()
                .FromInstance(shopWindow.GetComponentInChildren<ScrollUI>())
                .AsSingle();

            Container
                .Bind<RewardedAdItem>()
                .FromInstance(shopWindow.GetComponentInChildren<RewardedAdItem>())
                .AsSingle();

            Container.BindInterfacesAndSelfTo<Shop>().AsSingle();
        }

        private void BindSettings()
        {
            MenuSettings menuSettings = Container
                .InstantiatePrefabForComponent<MenuSettings>(_menuSettingsPrefab, _uiRoot);

            Container
                .BindInterfacesAndSelfTo<MenuSettings>()
                .FromInstance(menuSettings)
                .AsSingle();
        }

        private void BindTasks()
        {
            Container.BindInterfacesAndSelfTo<TaskCreator>().AsSingle();
            
            TaskUI taskUI = Container
                .InstantiatePrefabForComponent<TaskUI>(_taskUIPrefab, _menuWindow.transform);

            Container
                .Bind<TaskUI>()
                .FromInstance(taskUI)
                .AsSingle();
        }

        private void BindMenu()
        {
            Container
                .BindInterfacesAndSelfTo<Menu>()
                .AsSingle()
                .NonLazy();
        }
    }
}