using System;
using UnityEngine;
using UnityEngine.Advertisements;
using Zenject;

namespace CodeBase.Infrastructure.Services
{
    public class AdsService : IUnityAdsListener, IInitializable
    {
        private const string _androidGameId = "4731101";
        private const string _rewardedVideoAndroidId = "Rewarded_Android";

        private string _rewardedVideoID;
        private string _gameId;
        private Action _onVideoFinished;

        public event Action RewardedVideoReady;
        public int Reward => 25;

        public void Initialize()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    _rewardedVideoID = _rewardedVideoAndroidId;
                    _gameId = _androidGameId;
                    break;
                case RuntimePlatform.WindowsEditor:
                    _rewardedVideoID = _rewardedVideoAndroidId;
                    _gameId = _androidGameId;
                    break;
                default:
                    Debug.Log("Unsupported platform for ADs");
                    break;
            }
            
            Advertisement.AddListener(this);
            Advertisement.Initialize(_gameId);
        }

        public void ShowRewardedVideo(Action onVideoFinished)
        {
            Advertisement.Show(_rewardedVideoID);
            _onVideoFinished = onVideoFinished;
        }

        public bool IsRewardedVideoReady() => 
            Advertisement.IsReady(_rewardedVideoID);

        public void OnUnityAdsReady(string placementId)
        {
            if (placementId == _rewardedVideoID)
                RewardedVideoReady?.Invoke();
        }

        public void OnUnityAdsDidError(string message) => 
            Debug.Log($"OnUnityAdsDidError {message}");

        public void OnUnityAdsDidStart(string placementId) {}

        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            switch (showResult)
            {
                case ShowResult.Failed:
                    Debug.LogError($"OnUnityAdsDidFinish {showResult}");
                    break;
                case ShowResult.Skipped:
                    Debug.LogError($"OnUnityAdsDidFinish {showResult}");
                    break;
                case ShowResult.Finished:
                    _onVideoFinished?.Invoke();
                    break;
                default:
                    Debug.LogError($"OnUnityAdsDidFinish {showResult}");
                    break;
            }

            _onVideoFinished = null;
        }
    }
}