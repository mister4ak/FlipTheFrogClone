using System;
using System.Collections;
using CodeBase.CameraLogic;
using CodeBase.Coins;
using CodeBase.Colliders;
using CodeBase.Frog;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Services;
using CodeBase.UI;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.StateMachine.States
{
    public class GameOverState : IState, IInitializable
    {
        private readonly LevelUI _levelUI;
        private readonly FrogPlayer _frogPlayer;
        private readonly FrogCameraFollower _frogCamera;
        private readonly StaticDataService _staticData;
        private readonly DeadZone _deadZone;
        private readonly CoroutineHelper _coroutineHelper;
        private readonly LevelCreator _levelCreator;
        private Coin[] _coins;

        public event Action LevelRestarted;

        public GameOverState(
            LevelUI levelUI,
            FrogPlayer frogPlayer,
            FrogCameraFollower frogCamera,
            StaticDataService staticData,
            DeadZone deadZone,
            CoroutineHelper coroutineHelper,
            LevelCreator levelCreator
        )
        {
            _levelUI = levelUI;
            _frogPlayer = frogPlayer;
            _frogCamera = frogCamera;
            _staticData = staticData;
            _deadZone = deadZone;
            _coroutineHelper = coroutineHelper;
            _levelCreator = levelCreator;
        }

        public void Initialize()
        {
            _levelUI.LevelRestart += OnLevelRestart;
            _levelCreator.LevelCreated += (levelData) => _coins = levelData.coins;
        }

        private void OnLevelRestart() => 
            _coroutineHelper.StartCoroutine(RestartLevel());

        public void OnEnter() => 
            _levelUI.OpenGameOverWindow();

        public void OnExit() {}

        private IEnumerator RestartLevel()
        {
            _deadZone.gameObject.SetActive(false);
            yield return new WaitForSeconds(1);
            
            _frogPlayer.Reset(_staticData.PlayerData().startPoint.position);
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