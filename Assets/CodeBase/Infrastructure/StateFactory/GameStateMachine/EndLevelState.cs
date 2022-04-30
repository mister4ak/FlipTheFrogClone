using System;
using System.Collections;
using CodeBase.Colliders;
using CodeBase.Data;
using CodeBase.Frog;
using CodeBase.GameCamera;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.UI;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.StateFactory.GameStateMachine
{
    public class EndLevelState : IState, IInitializable, ISavedProgress
    {
        private readonly CoroutineHelper _coroutineHelper;
        private readonly TimeService _timeService;
        private readonly LevelUI _levelUI;
        private readonly DeadZone _deadZone;
        private readonly CinemachineSwitcher _cinemachineSwitcher;
        private readonly FrogPlayer _frogPlayer;
        private readonly Transform _belowMoonTransform;
        private readonly Transform _moonTransform;
        private FrogCameraFollower _frogCamera;
        private PlayerProgress _progress;
        private StaticDataService _staticData;
        private const float MoveDuration = 1f;

        public event Action LevelEnded;
        
        public EndLevelState(
            CoroutineHelper coroutineHelper,
            LevelUI levelUI,
            TimeService timeService,
            DeadZone deadZone,
            CinemachineSwitcher cinemachineSwitcher,
            FrogPlayer frogPlayer,
            FrogCameraFollower frogCamera,
            StaticDataService staticData,
            Transform belowMoonTransform,
            Transform moonTransform
        )
        {
            _staticData = staticData;
            _frogCamera = frogCamera;
            _moonTransform = moonTransform;
            _belowMoonTransform = belowMoonTransform;
            _frogPlayer = frogPlayer;
            _cinemachineSwitcher = cinemachineSwitcher;
            _deadZone = deadZone;
            _levelUI = levelUI;
            _timeService = timeService;
            _coroutineHelper = coroutineHelper;
        }

        public void Initialize() => 
            _levelUI.NextLevel += MoveToNextLevel;

        public void OnEnter()
        {
            _coroutineHelper.StartCoroutine(EndLevelProcess());
        }

        private IEnumerator EndLevelProcess()
        {
            _progress.PlayerData.currentLevelIndex++;

            _deadZone.gameObject.SetActive(false);
            if (_timeService.IsTimeSlowDown == false)
                _timeService.SlowDown();
            
            yield return new WaitForSecondsRealtime(1f);
            _cinemachineSwitcher.SwitchState();
            _timeService.Resume();

            yield return new WaitForSeconds(1f);
            _frogPlayer.Move(_belowMoonTransform.position, _moonTransform.position, MoveDuration);
            
            yield return new WaitForSeconds(1f);
            _levelUI.OpenEndLevelWindow();
        }

        private void MoveToNextLevel()
        {
            _frogPlayer.ResetFrog(_staticData.PlayerData().StartPoint.position);
            _frogCamera.ResetPosition();
            
            _cinemachineSwitcher.SwitchState();
            _deadZone.gameObject.SetActive(true);
            
            LevelEnded?.Invoke();
        }

        public void OnExit() {}

        public void LoadProgress(PlayerProgress progress) => 
            _progress = progress;

        public void UpdateProgress(PlayerProgress progress) => 
            progress.PlayerData.currentLevelIndex = _progress.PlayerData.currentLevelIndex;
    }
}