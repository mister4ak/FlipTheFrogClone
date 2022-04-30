using CodeBase.Data;
using CodeBase.Infrastructure.Services.SaveLoad;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Services.PersistentProgress
{
    public class PersistentProgressService
    {
        public PlayerProgress Progress { get; set; }
    }
}