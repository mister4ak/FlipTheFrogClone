using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.UI
{
    public class CoinText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _coinText;
        private PlayerData _playerData;
        private PersistentProgressService _progressService;

        [Inject]
        private void Construct(PersistentProgressService progressService)
        {
            Debug.Log("Coin text construct");
            _progressService = progressService;
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            Debug.Log("Coin text created");
            Debug.Log(_progressService.Progress);
            _playerData = _progressService.Progress.PlayerData;
            _playerData.CoinsChanged += UpdateCoins;
            UpdateCoins();
        }

        // public void Initialize()
        // {
        //     Debug.Log("Coin text created");
        //     _playerData = _progressService.Progress.PlayerData;
        //     _playerData.CoinsChanged += UpdateCoins;
        //     UpdateCoins();
        // }

        private void UpdateCoins() => 
            _coinText.text = _playerData.coins.ToString();
    }
}
