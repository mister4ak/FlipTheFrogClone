using System;
using CodeBase.Frog;
using UnityEngine;

namespace CodeBase.Colliders
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class FinishLine : MonoBehaviour
    {
        public event Action OnFinish;
        private BoxCollider2D _collider;

        private void Start() => 
            _collider = GetComponent<BoxCollider2D>();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out FrogPlayer frog))
            {
                _collider.enabled = false;
                OnFinish?.Invoke();
            }
        }
    }
}
