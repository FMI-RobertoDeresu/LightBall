using System.Collections;
using System.Linq;
using Assets.Scripts.Services;
using UnityEngine;

namespace Assets.Scripts.Modules
{
    public class Startup : SingletonScript<Startup>
    {
        public GameObject[] prefabs;

        private void Awake()
        {
            var isFirstInstance = SetInstance();
            if (isFirstInstance)
                OnStart();
        }

        private void OnStart()
        {
            AppManager.Create(prefabs);
            //StartCoroutine(DebugInfo());
        }

        public IEnumerator DebugInfo()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                Debug.Log("Frame rate: " + 1.0f / Time.deltaTime);
            }
        }
    }
}