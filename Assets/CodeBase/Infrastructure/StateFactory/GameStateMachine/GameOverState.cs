using System;
using System.Collections;
using CodeBase.Coins;
using CodeBase.Colliders;
using CodeBase.Frog;
using CodeBase.GameCamera;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Services;
using CodeBase.UI;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.StateFactory.GameStateMachine
{
    public class GameOverState : IState, IInitializable
    {
        private LevelUI _levelUI;
        private FrogPlayer _frogPlayer;
        private StaticDataService _staticData;
        private FrogCameraFollower _frogCamera;
        private DeadZone _deadZone;
        private CoroutineHelper _coroutineHelper;
        private LevelCreator _levelCreator;
        private Coin[] _coins;

        public event Action LevelRestarted;

        [Inject]
        private void Construct(
            LevelUI levelUI,
            FrogPlayer frogPlayer,
            FrogCameraFollower frogCamera,
            StaticDataService staticData,
            DeadZone deadZone,
            CoroutineHelper coroutineHelper,
            LevelCreator levelCreator
        )
        {
            _levelCreator = levelCreator;
            _coroutineHelper = coroutineHelper;
            _deadZone = deadZone;
            _frogCamera = frogCamera;
            _staticData = staticData;
            _frogPlayer = frogPlayer;
            _levelUI = levelUI;
        }

        public void Initialize()
        {
            _levelUI.LevelRestart += OnLevelRestart;
            _levelCreator.LevelCreated += (levelData) => _coins = levelData.coins;
        }

        private void OnLevelRestart() => 
            _coroutineHelper.StartCoroutine(RestartLevel());

        public void OnEnter()
        {
            _levelUI.OpenGameOverWindow();
        }

        public void OnExit() { }

        private IEnumerator RestartLevel()
        {
            _deadZone.gameObject.SetActive(false);
            yield return new WaitForSeconds(1);
            
            _frogPlayer.ResetFrog(_staticData.PlayerData().StartPoint.position);
            _frogCamera.ResetPosition();
            
            ResetCoinsActiveState();
            _deadZone.gameObject.SetActive(true);
            
            LevelRestarted?.Invoke();
        }

        private void ResetCoinsActiveState()
        {
            foreach (var coin in _coins)
                if (coin.isActiveAndEnabled == false)
                    coin.SetActiveState(true);
        }
    }
}