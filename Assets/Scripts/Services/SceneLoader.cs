using System.Collections;
using Assets.Scripts.Modules;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Services
{
    public class SceneLoader : SingletonServiceBase<SceneLoader>
    {
        private GameObject _loader;

        public static SceneLoader Create()
        {
            return CreateInstance();
        }

        public IEnumerator LoadScene(string sceneName, bool showLoader = true)
        {
            var loaderManager = (LoaderManager)null;
            if (showLoader)
            {
                _loader = Instantiate(AppManager.Instance.PrefabsService.LoaderFadeInPrefab);
                DontDestroyOnLoad(_loader);
                loaderManager = _loader.GetComponent<LoaderManager>();
            }

            var loadSceneAsync = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            loadSceneAsync.allowSceneActivation = false;

            while (loadSceneAsync.isDone == false)
            {
                if (showLoader)
                {
                    var loadingProgress = Mathf.Clamp01(loadSceneAsync.progress / 0.9f);
                    loaderManager.UpdateProgress(loadingProgress);
                }

                if (loadSceneAsync.progress >= 0.9f)
                    break;

                yield return null;
            }

            loadSceneAsync.allowSceneActivation = true;
        }

        public void SceneIsReady()
        {
            if (_loader != null)
            {
                var loaderManager = _loader.GetComponent<LoaderManager>();
                loaderManager.FadeIn(null);
            }
        }
    }
}