using System;
using CodeBase.Coins;
using CodeBase.Colliders;
using CodeBase.Data;
using CodeBase.Frog;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.StaticData;

namespace CodeBase.Tasks
{
    public class TaskTracker : ISavedProgress
    {
        private readonly StaticDataService _staticDataService;
        private readonly FrogMover _frogMover;
        private readonly LevelCreator _levelCreator;
        
        private Task _currentTask;
        private TaskId _currentTaskId;
        private Coin[] _coins;
        private FinishLine _finishLine;
        
        private int _taskProgress;
        private bool _isTaskCompleted;
        private bool _isRewardReceived;
        
        private Action<Coin> _collectCoinAction;

        public TaskTracker(
            StaticDataService staticDataService,
            FrogMover frogMover,
            LevelCreator levelCreator)
        {
            _staticDataService = staticDataService;
            _frogMover = frogMover;
            _levelCreator = levelCreator;
        }

        public void StartTaskTrackingIfHas()
        {
            if (_isTaskCompleted)
                return;
            _currentTask = _staticDataService.ForTask(_currentTaskId);
            SubscribeForTaskAction();
        }

        private void SubscribeForTaskAction()
        {
            switch (_currentTaskId)
            {
                case TaskId.CollectCoins:
                    CollectCoins();
                    break;
                case TaskId.FinishLevel:
                    FinishLevel();
                    break;
                case TaskId.Jump:
                    Jump();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void CollectCoins()
        {
            _collectCoinAction = (_) => TaskProgressed();
            _coins = _levelCreator.GetData().coins;
            foreach (Coin coin in _coins)
                coin.CoinPicked += _collectCoinAction;
        }

        private void FinishLevel()
        {
            _finishLine = _levelCreator.GetData().finishLine;
            _finishLine.OnFinish += TaskProgressed;
        }

        private void Jump() => 
            _frogMover.Jumped += TaskProgressed;

        private void TaskProgressed()
        {
            _taskProgress++;
            if (_taskProgress == _currentTask.conditionNumber)
                EndTask();
        }

        private void EndTask()
        {
            _isTaskCompleted = true;
            UnsubscribeCurrentTaskAction();
        }

        private void UnsubscribeCurrentTaskAction()
        {
            switch (_currentTaskId)
            {
                case TaskId.CollectCoins:
                    UnsubscribeCollectCoins();
                    break;
                case TaskId.FinishLevel:
                    UnsubscribeFinishLevel();
                    break;
                case TaskId.Jump:
                    UnsubscribeJump();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UnsubscribeCollectCoins()
        {
            foreach (Coin coin in _coins)
                coin.CoinPicked -= _collectCoinAction;
        }

        private void UnsubscribeFinishLevel() => 
            _finishLine.OnFinish -= TaskProgressed;

        private void UnsubscribeJump() => 
            _frogMover.Jumped -= TaskProgressed;

        public void LoadProgress(PlayerProgress progress)
        {
            _currentTaskId = progress.TaskData.taskId;
            _taskProgress = progress.TaskData.progress ;
            _isTaskCompleted = progress.TaskData.isCompleted;
            _isRewardReceived = progress.TaskData.isRewardReceived;
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.TaskData.taskId = _currentTaskId;
            progress.TaskData.progress = _taskProgress;
            progress.TaskData.isCompleted = _isTaskCompleted;
            progress.TaskData.isRewardReceived = _isRewardReceived;
        }
    }
    
}