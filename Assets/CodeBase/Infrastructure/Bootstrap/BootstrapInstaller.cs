using CodeBase.Audio;
using CodeBase.Infrastructure.AssetManagment;
using CodeBase.Infrastructure.Services.Input;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Infrastructure.Services.StaticData;
using CodeBase.UI;
using CodeBase.UI.Windows;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Bootstrap
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _hudPrefab;
        [SerializeField] private CrossfadeWindow _crossfadeCanvas;
        [SerializeField] private CoroutineHelper _coroutineHelperPrefab;
        [SerializeField] private AudioPlayer _audioPlayerPrefab;

        public override void InstallBindings()
        {
            BindCoroutineHelper();
            BindProgressService();
            BindSaveLoadService();
            BindSceneLoader();
            BindCrossfadeCanvas();
            BindAssetProvider();
            BindHud();
            BindAudioManager();
            BindStaticDataService();
        }

        private void BindCoroutineHelper()
        {
            CoroutineHelper coroutineHelper = Container
                            .InstantiatePrefabForComponent<CoroutineHelper>(_coroutineHelperPrefab);
            
            Container
                .Bind<CoroutineHelper>()
                .FromInstance(coroutineHelper)
                .AsSingle();
        }

        private void BindStaticDataService()
        {
            Container
                .Bind<StaticDataService>()
                .AsSingle();
        }

        private void BindAudioManager()
        {
            AudioPlayer audioPlayer = Container
                .InstantiatePrefabForComponent<AudioPlayer>(_audioPlayerPrefab);

            Container
                .Bind<AudioPlayer>()
                .FromInstance(audioPlayer)
                .AsSingle();
        }

        private void BindHud()
        {
            CoinText hud = Container
                .InstantiatePrefabForComponent<CoinText>(_hudPrefab);

            Container
                .Bind<CoinText>()
                .FromInstance(hud)
                .AsSingle();
        }

        private void BindCrossfadeCanvas()
        {
            CrossfadeWindow crossfadeCanvas = Container
                .InstantiatePrefabForComponent<CrossfadeWindow>(_crossfadeCanvas);

            Container
                .Bind<CrossfadeWindow>()
                .FromInstance(crossfadeCanvas)
                .AsSingle()
                .NonLazy();
        }

        private void BindSceneLoader()
        {
            Container
                .Bind<SceneLoader>()
                .AsSingle();
        }

        private void BindProgressService()
        {
            Container
                .Bind<PersistentProgressService>()
                .AsSingle();
            
        }

        private void BindSaveLoadService()
        {
            Container
                .Bind<SaveLoadService>()
                .AsSingle();
        }

        private void BindInputService()
        {
            if (Application.isEditor)
                Container.Bind<MouseInput>().AsSingle();
            else
                Container.Bind<TouchInput>().AsSingle();
        }

        private void BindAssetProvider()
        {
            Container
                .Bind<AssetProvider>()
                .AsSingle();
        }
        
        
    }
}