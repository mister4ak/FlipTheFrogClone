﻿using System.Collections.Generic;
using System.Linq;
using CodeBase.StaticData;
using CodeBase.Tasks;
using UnityEngine;

namespace CodeBase.Infrastructure.Services
{
    public class StaticDataService
    {
        private const string PlayerDataPath = "StaticData/Player/PlayerData";
        private const string TasksDataPath = "StaticData/Tasks";

        private Dictionary<TaskId, Task> _levels;
        private PlayerStaticData _playerData;

        public void Load()
        {
            _playerData = Resources.Load<PlayerStaticData>(PlayerDataPath);
            
            _levels = Resources
                .LoadAll<Task>(TasksDataPath)
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