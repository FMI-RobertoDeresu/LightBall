using Assets.Scripts.ModuleModels.Menu;
using Assets.Scripts.Services;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Modules.Menu
{
    public class Level : MonoBehaviour
    {
        private int _index;

        public GameObject textGo;
        public GameObject starsGo;
        public GameObject playButtonGo;

        public void Show(int index, LevelItem level)
        {
            _index = index;

            var text = textGo.GetComponent<Text>();
            text.text = level.Name;

            var levelStarsManager = starsGo.GetComponent<LevelStarsManager>();
            levelStarsManager.ShowStars(level.Stars);

            if (!level.Available)
            {
                var button = GetComponent<Button>();
                button.interactable = false;

                var image = GetComponent<Image>();
                image.color += Color.black * -0.2f;

                text.color += Color.black * -0.5f;

                levelStarsManager.ShowStarsAsDisabled();
                Destroy(playButtonGo);
            }
        }

        public void Play()
        {
            AppManager.Instance.AppContext.LevelToPlay = _index;
            StartCoroutine(AppManager.Instance.SceneLoader.LoadScene(SceneNames.Level));
        }
    }
}
