using UnityEngine;

namespace CodeBase.Tasks
{
    [CreateAssetMenu(fileName = "NewTask", menuName = "Static Data/Task")]
    public class TaskScriptable : ScriptableObject
    {
        public TaskId taskId;

        public string description;
        
        public Sprite sprite;
        
        [Range(1,100)]
        public int conditionNumber;
        
        [Range(1,100)]
        public int reward;
    }
}