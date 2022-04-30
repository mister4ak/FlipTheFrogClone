using System;
using CodeBase.Tasks;

namespace CodeBase.Data
{
    [Serializable]
    public class TaskData
    {
        public TaskId taskId;
        public int progress;
        public bool isCompleted;
        public bool isRewardReceived;
    }
}