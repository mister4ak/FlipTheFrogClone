﻿using System.Collections.Generic;
using System.Linq;
using CodeBase.MainMenu.PlayerShop;
using CodeBase.StaticData;
using CodeBase.Tasks;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.StaticData
{
    public class StaticDataService
    {
        private const string _playerDataPath = "StaticData/Player/PlayerData";
        private const string _tasksDataPath = "StaticData/Tasks";

        private Dictionary<TaskId, TaskScriptable> _levels;
        private PlayerStaticData _playerData;

        public void Load()
        {
            _playerData = Resources.Load<PlayerStaticData>(_playerDataPath);
            
            _levels = Resources
                .LoadAll<TaskScriptable>(_tasksDataPath)
                .ToDictionary(x => x.taskId, x => x);
        }

        public PlayerStaticData PlayerData() => 
            _playerData;

        public TaskScriptable ForTask(TaskId taskId) =>
            _levels.TryGetValue(taskId, out TaskScriptable taskData)
                ? taskData
                : null;

    }
}