using UnityEngine;

namespace Assets.Scripts.Modules
{
    public class ExceptionHandler : SingletonScript<ExceptionHandler>
    {
        private void Awake()
        {
            var isFirstInstance = SetInstance();
            if (isFirstInstance)
                Application.logMessageReceived += ApplicationOnLogMessageReceived;
        }

        private void ApplicationOnLogMessageReceived(string condition, string stackTrace, LogType type)
        {
            //if (type == LogType.Exception)
            Debug.Log($"{type}: {condition}\n{stackTrace}");
        }
    }
}