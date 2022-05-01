using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Static Data/Player")]
    public class PlayerStaticData: ScriptableObject
    {
        [Range(1,20)]
        public float forceLaunch;

        [Range(1, 40)]
        public float trampolineForceLaunch;

        [Range(1,100)]
        public float maxDragDistance;

        public Transform startPoint;
    }
}