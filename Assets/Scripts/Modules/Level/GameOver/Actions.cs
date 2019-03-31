using Assets.Scripts.Services;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Modules.Level.GameOver
{
    public class Actions : MonoBehaviour
    {
        public void OnHomeButtonClick()
        {
            StartCoroutine(AppManager.Instance.SceneLoader.LoadScene(SceneNames.Menu));
        }

        public void OnRestartButtonClick()
        {
            StartCoroutine(AppManager.Instance.SceneLoader.LoadScene(SceneNames.Level));
        }

        public void OnShareButtonClick()
        {
            Debug.Log("Share");
        }
    }
}