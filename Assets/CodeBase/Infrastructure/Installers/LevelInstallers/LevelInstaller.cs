using CodeBase.Coins;
using CodeBase.Colliders;
using CodeBase.Frog;
using CodeBase.GameCamera;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.StateMachine;
using CodeBase.Infrastructure.StateMachine.States;
using CodeBase.Tasks;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Installers.LevelInstallers
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private Transform _startPoint;
        [SerializeField] private GameObject _frogPrefab;
        [SerializeField] private GameObject _frogCamera;
        [SerializeField] private DeadZone _deadZone;
        [SerializeField] private ParticleEmmiter _particleEmmiterPrefab;
        [SerializeField] private Transform _moonTransform;
        [SerializeField] private Transform _belowMoonTransform;

        public override void InstallBindings()
        {
            BindTimeService();
            BindLevelCreator();
            BindGameStateMachine();
            BindParticleEmmiter();
            BindFrog();
            BindCoinCollisionHandler();
            BindCamera();
            BindDeadZone();
            BindTaskTracker();
        }

        private void BindTaskTracker()
        {
            Container
                .BindInterfacesAndSelfTo<TaskTracker>()
                .AsSingle();
        }

        private void BindLevelCreator()
        {
            Container.Bind<LevelCreator>().AsSingle();
            Container.BindFactory<int, Level, Level.Factory>().AsSingle();
        }

        private void BindTimeService() => 
            Container.Bind<TimeService>().AsSingle();

        private void BindCamera()
        {
            Container.Bind<FrogCameraFollower>().FromComponentOn(_frogCamera).AsSingle();
            Container.Bind<CinemachineSwitcher>().FromComponentOn(_frogCamera).AsSingle();
        }

        private void BindDeadZone() => 
            Container.Bind<DeadZone>().FromInstance(_deadZone).AsSingle();

        private void BindCoinCollisionHandler() => 
            Container.Bind<CoinCollisionHandler>().AsSingle();

        private void BindParticleEmmiter()
        {
            ParticleEmmiter particleEmmiter = Container
                .InstantiatePrefabForComponent<ParticleEmmiter>(_particleEmmiterPrefab);

            Container.Bind<ParticleEmmiter>().FromInstance(particleEmmiter).AsSingle();
        }

        private void BindGameStateMachine()
        {
            Container.BindInterfacesAndSelfTo<LoadLevelState>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameLoopState>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameOverState>().AsSingle();
            Container.BindInterfacesAndSelfTo<EndLevelState>().AsSingle().WithArguments(_belowMoonTransform, _moonTransform);
            
            Container.BindInterfacesAndSelfTo<Game>().AsSingle().NonLazy();
        }
        
        private void BindFrog()
        {
            FrogPlayer frogPlayer = Container
                .InstantiatePrefabForComponent<FrogPlayer>(_frogPrefab, _startPoint.position, Quaternion.identity, null);

            Container.BindInterfacesAndSelfTo<FrogPlayer>().FromInstance(frogPlayer).AsSingle();
            Container.Bind<FrogMover>().FromInstance(frogPlayer.GetComponent<FrogMover>()).AsSingle();
            
            Container.BindInterfacesAndSelfTo<FrogCollisionHandler>().AsSingle();
        }
        
    }
}