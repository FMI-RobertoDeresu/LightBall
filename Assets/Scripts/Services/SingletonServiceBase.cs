using System;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class SingletonServiceBase<T> : MonoBehaviour
        where T : SingletonServiceBase<T>
    {
        private static bool _shuttingDown;
        private static readonly object _lock = new object();

        private static T _instance;
        private static GameObject _singletonObject;

        public static bool InstanceCreated => _instance != null;

        public static T Instance
        {
            get
            {
                if (_shuttingDown)
                    return null;

                if (_instance == null)
                    throw new Exception($"No instance of type '{typeof(T)}'.");

                return _instance ?? (T) FindObjectOfType(typeof(T));
            }
        }

        protected static T CreateInstance()
        {
            lock (_lock)
            {
                if (_instance != null)
                    throw new Exception($"Existing instance of type '{typeof(T)}'.");

                _singletonObject = new GameObject();
                _singletonObject.name = typeof(T) + " (Singleton)";
                DontDestroyOnLoad(_singletonObject);

                _instance = _singletonObject.AddComponent<T>();
                return _instance;
            }
        }

        private void Awake()
        {
            if (gameObject != _singletonObject)
                throw new NotImplementedException($"{typeof(T)}");
        }

        private void OnApplicationQuit()
        {
            _shuttingDown = true;
        }

        private void OnDestroy()
        {
            _shuttingDown = true;
        }
    }
}