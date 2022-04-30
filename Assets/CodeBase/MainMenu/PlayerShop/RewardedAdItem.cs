using CodeBase.Infrastructure.Services.Ads;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.MainMenu.PlayerShop
{
    public class RewardedAdItem : MonoBehaviour
    {
        private AdsService _adsService;
        
        [SerializeField] private Button _showAdButton;
        [SerializeField] private GameObject[] _adActiveObjects;
        [SerializeField] private GameObject[] _adInactiveObjects;
        private PersistentProgressService _progressService;

        [Inject]
        private void Construct(AdsService adsService, PersistentProgressService progressService)
        {
            _progressService = progressService;
            _adsService = adsService;
        }
        
        public void Initialize()
        {
            _showAdButton.onClick.AddListener(OnShowAdClicked);
            RefreshAvailableAd();
        }

        public void Subscribe() => 
            _adsService.RewardedVideoReady += RefreshAvailableAd;

        public void Cleanup() => 
            _adsService.RewardedVideoReady -= RefreshAvailableAd;

        private void OnShowAdClicked() => 
            _adsService.ShowRewardedVideo(OnVideoFinished);

        private void OnVideoFinished() => 
            _progressService.Progress.PlayerData.AddCoins(_adsService.Reward);

        private void RefreshAvailableAd()
        {
            bool isVideoReady = _adsService.IsRewardedVideoReady();

            foreach (GameObject adActiveObject in _adActiveObjects) 
                adActiveObject.SetActive(isVideoReady);

            foreach (GameObject adInactiveObject in _adInactiveObjects) 
                adInactiveObject.SetActive(!isVideoReady);
        }
    }
}