using CodeBase.Audio;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Input;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.UI;
using CodeBase.UI.Windows;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private CoinText _coinHudPrefab;
        [SerializeField] private CrossfadeWindow _crossfadeCanvas;
        [SerializeField] private CoroutineHelper _coroutineHelperPrefab;
        [SerializeField] private AudioPlayer _audioPlayerPrefab;

        public override void InstallBindings()
        {
            BindServices();
            BindCoroutineHelper();
            BindSceneLoader();
            BindCrossfadeCanvas();
            BindHud();
            BindAudioPlayer();
        }

        private void BindServices()
        {
            Container.Bind<PersistentProgressService>().AsSingle();
            Container.Bind<SaveLoadService>().AsSingle();
            Container.Bind<StaticDataService>().AsSingle();
            Container.BindInterfacesAndSelfTo<MouseInput>().AsSingle();
        }

        private void BindCoroutineHelper()
        {
            CoroutineHelper coroutineHelper = Container
                            .InstantiatePrefabForComponent<CoroutineHelper>(_coroutineHelperPrefab);
            
            Container.Bind<CoroutineHelper>().FromInstance(coroutineHelper).AsSingle();
        }

        private void BindSceneLoader() => 
            Container.Bind<SceneLoader>().AsSingle();

        private void BindCrossfadeCanvas()
        {
            CrossfadeWindow crossfadeCanvas = Container
                .InstantiatePrefabForComponent<CrossfadeWindow>(_crossfadeCanvas);

            Container.Bind<CrossfadeWindow>().FromInstance(crossfadeCanvas).AsSingle().NonLazy();
        }

        private void BindHud()
        {
            CoinText hud = Container
                .InstantiatePrefabForComponent<CoinText>(_coinHudPrefab);

            Container.BindInterfacesAndSelfTo<CoinText>().FromInstance(hud).AsSingle();
        }

        private void BindAudioPlayer()
        {
            AudioPlayer audioPlayer = Container
                .InstantiatePrefabForComponent<AudioPlayer>(_audioPlayerPrefab);

            Container.Bind<AudioPlayer>().FromInstance(audioPlayer).AsSingle();
        }
    }
}