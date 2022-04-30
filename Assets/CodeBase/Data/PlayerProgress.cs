using System;

namespace CodeBase.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public TaskData TaskData;
        public PlayerData PlayerData;
        public SkinData SkinData;
        public PlayerSettings PlayerSettings; 

        public PlayerProgress()
        {
            TaskData = new TaskData();
            PlayerData = new PlayerData();
            SkinData = new SkinData();
            PlayerSettings = new PlayerSettings();
        }
    }
}