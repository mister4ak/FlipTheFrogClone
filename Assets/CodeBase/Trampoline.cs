using UnityEngine;

namespace CodeBase
{
    public class Trampoline : MonoBehaviour
    {
        [SerializeField] private Transform _perpendicularPoint;

        public Vector2 GetLaunchDirection() => 
            (_perpendicularPoint.position - transform.position).normalized;

        public Vector2 GetJumpPosition() => 
            transform.position;
    }
}
