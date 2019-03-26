using System;
using System.Collections;
using Assets.Scripts.Services;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Modules.Loader
{
    public class LoaderSceneManager : MonoBehaviour
    {
        private bool _ready;

        private void Awake()
        {
            StartCoroutine(CoroutineUtils.WaitUntil(() => AppManager.InstanceCreated, () => _ready = true));
        }
        
        private IEnumerator Start()
        {
            yield return new WaitUntil(() => _ready);
            yield return AppManager.Instance.SceneLoader.LoadScene(SceneNames.Menu);
        }
    }
}