using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.StaticData;
using Random = System.Random;

namespace CodeBase.Tasks
{
    public class TaskCreator : ISavedProgress
    {
        private readonly StaticDataService _staticDataService;
        private readonly TaskUI _taskUI;
        
        private TaskId _currentTaskId;
        private Task _currentTask;
        private PlayerData _playerData;

        private int _taskProgress;
        private bool _isTaskCompleted;
        private bool _isRewardReceived;

        public TaskCreator(StaticDataService staticDataService, TaskUI taskUI)
        {
            _taskUI = taskUI;
            _staticDataService = staticDataService;
        }

        public void Initialize()
        {
            if (_isTaskCompleted && _isRewardReceived)
                GenerateRandomTask();
            _currentTask = _staticDataService.ForTask(_currentTaskId);
            _taskUI.Initialize(_currentTask, _isTaskCompleted, _taskProgress);
            _taskUI.RewardReceived += ReceiveReward;
        }

        private void ReceiveReward()
        {
            _isRewardReceived = true;
            _playerData.AddCoins(_currentTask.reward);
            _taskUI.RewardReceived -= ReceiveReward;
        }

        private void GenerateRandomTask()
        {
            GenerateTask();
            ResetCurrentTaskData();
        }

        private void GenerateTask()
        {
            Array enumValues = Enum.GetValues(typeof(TaskId));
            Random random = new Random();
            _currentTaskId = (TaskId) enumValues.GetValue(random.Next(enumValues.Length));
        }

        private void ResetCurrentTaskData()
        {
            _taskProgress = 0;
            _isTaskCompleted = false;
            _isRewardReceived = false;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _playerData = progress.PlayerData;

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