using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Infrastructure.Services
{
    public class SaveLoadService
    {
        private readonly PersistentProgressService _progressService;
        private const string ProgressKey = "Progress";

        public SaveLoadService(PersistentProgressService progressService) => 
            _progressService = progressService;

        public void SaveProgress(ISavedProgress[] progressWriters)
        {
            foreach (ISavedProgress progressWriter in progressWriters)
                progressWriter.UpdateProgress(_progressService.Progress);
        
            PlayerPrefs.SetString(ProgressKey, _progressService.Progress.ToJson());
        }

        public PlayerProgress LoadProgress() =>
            PlayerPrefs.GetString(ProgressKey)?
                .ToDeserialized<PlayerProgress>();
    }
}