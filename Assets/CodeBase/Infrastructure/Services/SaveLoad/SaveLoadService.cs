using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.SaveLoad
{
    public class SaveLoadService
    {
        private readonly PersistentProgressService _progressService;
        private const string _progressKey = "Progress";

        public SaveLoadService(PersistentProgressService progressService)
        {
            _progressService = progressService;
        }
        
        public void SaveProgress(ISavedProgress[] progressWriters)
        {
            foreach (ISavedProgress progressWriter in progressWriters)
                progressWriter.UpdateProgress(_progressService.Progress);
        
            PlayerPrefs.SetString(_progressKey, _progressService.Progress.ToJson());
            Debug.Log("Progress saved");
        }

        public PlayerProgress LoadProgress() =>
            PlayerPrefs.GetString(_progressKey)?
                .ToDeserialized<PlayerProgress>();
    }
}