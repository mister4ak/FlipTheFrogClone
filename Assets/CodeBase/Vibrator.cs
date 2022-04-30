using UnityEngine;

namespace CodeBase
{
    public static class Vibrator
    {
        private static AndroidJavaObject _vibrator;
        private static bool _isInitialized;
        private static bool _isVibrationEnabled;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
#if UNITY_ANDROID
            if (Application.isConsolePlatform) { Handheld.Vibrate(); }
#endif

            if (_isInitialized == false && Application.platform == RuntimePlatform.Android)
            {
                using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                    if (currentActivity != null)
                        _vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");

                _isInitialized = true;
            }
        }

        public static void Vibrate(long milliseconds)
        {
            Initialize();
            if (_isInitialized && _isVibrationEnabled) 
                _vibrator.Call("vibrate", milliseconds);
        }

        public static void ChangeVibratorState(bool state) => 
            _isVibrationEnabled = state;
    }
}
