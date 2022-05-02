using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure
{
    public class SceneLoader
    {
        private readonly CoroutineHelper _coroutineHelper;
        private AsyncOperation _asyncOperation;

        public SceneLoader(CoroutineHelper coroutineHelper) => 
            _coroutineHelper = coroutineHelper;

        public void Load(string sceneName, Action onLoaded = null)
        {
            _coroutineHelper.StartCoroutine(LoadSceneCoroutine(sceneName, onLoaded));
        }

        private IEnumerator LoadSceneCoroutine(string sceneName, Action onLoaded = null)
        {
            _asyncOperation = SceneManager.LoadSceneAsync(sceneName);

            while (!_asyncOperation.isDone)
                yield return null;
            
            onLoaded?.Invoke();
        }
    }
}
