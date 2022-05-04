using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Infrastructure.AssetManagment;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Tasks;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure
{
    public class Boot : MonoBehaviour
    {
        private const string MainMenuScene = "MainMenu";
        private const string DefaultSkinName = "FrogOrange";
        
        private SceneLoader _sceneLoader;
        private PersistentProgressService _progressService;
        private SaveLoadService _saveLoadService;
        private StaticDataService _staticData;

        [Inject]
        public void Construct(
            StaticDataService staticData, 
            SaveLoadService saveLoadService, 
            PersistentProgressService progressService, 
            SceneLoader sceneLoader
            )
        {
            _staticData = staticData;
            _saveLoadService = saveLoadService;
            _progressService = progressService;
            _sceneLoader = sceneLoader;
        }

        private void Awake()
        {
            PlayerPrefs.DeleteAll();
            LoadStaticData();
            LoadProgressOrInitNew();
            _sceneLoader.Load(MainMenuScene);
        }

        private void LoadProgressOrInitNew() => 
            _progressService.Progress = _saveLoadService.LoadProgress() ?? NewProgress();

        private void LoadStaticData() => 
            _staticData.Load();

        private PlayerProgress NewProgress()
        {
            PlayerProgress progress = new PlayerProgress();
            
            InitPlayerData(progress);
            InitSkinData(progress);
            InitTaskData(progress);
            InitSettingsData(progress);

            return progress;
        }

        private void InitSettingsData(PlayerProgress progress) => 
            progress.PlayerSettings.isAudioPaused = false;

        private void InitTaskData(PlayerProgress progress)
        {
            progress.TaskData.taskId = TaskId.Jump;
            progress.TaskData.progress = 0;
            progress.TaskData.isCompleted = false;
            progress.TaskData.isRewardReceived = false;
        }

        private void InitSkinData(PlayerProgress progress)
        {
            progress.SkinData.purchasedSkins = new List<int>(1) {0};
            progress.SkinData.skinSpritePath = $"{AssetPath.FrogSprites}{DefaultSkinName}";
            progress.SkinData.currentSkinID = 0;
        }

        private void InitPlayerData(PlayerProgress progress)
        {
            progress.PlayerData.coins = 100;
            progress.PlayerData.currentLevelIndex = 1;
            progress.PlayerData.levelsCount = 7;
            progress.PlayerData.isTutorialCompleted = false;
        }
    }
}