using System;
using System.Collections;
using CodeBase.UI.Windows;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase
{
    public class SceneLoader
    {
        private readonly CoroutineHelper _coroutineHelper;
        private readonly CrossfadeWindow _crossfadeWindow;
        private AsyncOperation _asyncOperation;

        public SceneLoader(CoroutineHelper coroutineHelper, CrossfadeWindow crossfadeWindow)
        {
            _crossfadeWindow = crossfadeWindow;
            _coroutineHelper = coroutineHelper;
        }

        public void Load(string sceneName, bool waitForCrossfade = true)
        {
            _coroutineHelper.StartCoroutine(LoadSceneCoroutine(sceneName, waitForCrossfade));
            _crossfadeWindow.CrossfadeEnded += ActivateNextScene;
        }

        private IEnumerator LoadSceneCoroutine(string sceneName, bool waitForCrossfade = true)
        {
            _asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            _asyncOperation.allowSceneActivation = !waitForCrossfade;

            while (!_asyncOperation.isDone)
                yield return null;
        }

        private void ActivateNextScene() => 
            _asyncOperation.allowSceneActivation = true;
    }
}
