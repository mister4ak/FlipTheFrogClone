using System;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class CoroutineHelper : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
