using UnityEngine;

namespace Assets.Scripts.Modules {
    public abstract class SingletonScript<T> : MonoBehaviour
        where T : SingletonScript<T>
    {
        public static T Instance { get; private set; }

        protected bool SetInstance()
        {
            if (Instance == null)
            {
                Instance = (T) this;
                DontDestroyOnLoad(gameObject);
                return true;
            }
            else
            {
                Destroy(gameObject);
                return false;
            }
        }
    }
}