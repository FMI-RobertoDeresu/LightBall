using System.Linq;
using Assets.Scripts.Services;
using UnityEngine;

namespace Assets.Scripts.Modules
{
    public class Startup : MonoBehaviour
    {
        public GameObject[] prefabs;

        public static Startup Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                OnStart();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnStart()
        {
            AppManager.Create(prefabs);
        }
    }
}