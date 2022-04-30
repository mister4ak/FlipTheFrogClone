using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Static Data/Player")]
    public class PlayerStaticData: ScriptableObject
    {
        [Range(1,20)]
        public float ForceLaunch;

        [Range(1, 40)]
        public float TrampolineForceLaunch;

        [Range(1,100)]
        public float MaxDragDistance;

        public Transform StartPoint;
    }
}