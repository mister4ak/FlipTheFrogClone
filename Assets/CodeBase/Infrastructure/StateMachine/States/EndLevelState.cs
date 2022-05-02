using System;
using System.Collections;
using CodeBase.CameraLogic;
using CodeBase.Colliders;
using CodeBase.Data;
using CodeBase.Frog;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.UI;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.StateMachine.States
{
    public class EndLevelState : IState, IInitializable, ISavedProgress
    {
        private const float MoveDuration = 1f;
        
        private readonly CoroutineHelper _coroutineHelper;
        private readonly TimeService _timeService;
        private readonly LevelUI _levelUI;
        private readonly DeadZone _deadZone;
        private readonly CinemachineSwitcher _cinemachineSwitcher;
        private readonly FrogPlayer _frogPlayer;
        private readonly FrogCameraFollower _frogCamera;
        private readonly StaticDataService _staticData;
        private readonly Transform _belowMoonTransform;
        private readonly Transform _moonTransform;
        private PlayerProgress _progress;

        public event Action LevelEnded;
        
        public EndLevelState(
            CoroutineHelper coroutineHelper,
            TimeService timeService,
            LevelUI levelUI,
            DeadZone deadZone,
            CinemachineSwitcher cinemachineSwitcher,
            FrogPlayer frogPlayer,
            FrogCameraFollower frogCamera,
            StaticDataService staticData,
            Transform belowMoonTransform,
            Transform moonTransform
        )
        {
            _coroutineHelper = coroutineHelper;
            _timeService = timeService;
            _levelUI = levelUI;
            _deadZone = deadZone;
            _cinemachineSwitcher = cinemachineSwitcher;
            _frogPlayer = frogPlayer;
            _frogCamera = frogCamera;
            _staticData = staticData;
            _belowMoonTransform = belowMoonTransform;
            _moonTransform = moonTransform;
        }

        public void Initialize() => 
            _levelUI.NextLevel += MoveToNextLevel;

        public void OnEnter() => 
            _coroutineHelper.StartCoroutine(EndLevelProcess());

        private IEnumerator EndLevelProcess()
        {
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
            
            IncreaseCurrentLevelIndex();
        }

        private void IncreaseCurrentLevelIndex()
        {
            if (_progress.PlayerData.currentLevelIndex < _progress.PlayerData.levelsCount)
                _progress.PlayerData.currentLevelIndex++;
        }

        private void MoveToNextLevel()
        {
            _frogPlayer.ResetFrog(_staticData.PlayerData().startPoint.position);
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