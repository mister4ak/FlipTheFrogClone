using System.Collections.Generic;
using System.Linq;
using CodeBase.StaticData;
using CodeBase.Tasks;
using UnityEngine;

namespace CodeBase.Infrastructure.Services
{
    public class StaticDataService
    {
        private const string _playerDataPath = "StaticData/Player/PlayerData";
        private const string _tasksDataPath = "StaticData/Tasks";

        private Dictionary<TaskId, Task> _levels;
        private PlayerStaticData _playerData;

        public void Load()
        {
            _playerData = Resources.Load<PlayerStaticData>(_playerDataPath);
            
            _levels = Resources
                .LoadAll<Task>(_tasksDataPath)
                .ToDictionary(x => x.taskId, x => x);
        }

        public PlayerStaticData PlayerData() => 
            _playerData;

        public Task ForTask(TaskId taskId) =>
            _levels.TryGetValue(taskId, out Task taskData)
                ? taskData
                : null;

    }
}