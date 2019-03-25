using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Modules.Menu
{
    public class Level : MonoBehaviour
    {
        private int _index;

        public GameObject textGo;
        public GameObject starsGo;

        public void Show(int index, string name, int stars)
        {
            _index = index;

            var text = textGo.GetComponent<Text>();
            text.text = name;

            var levelStarsManager = starsGo.GetComponent<LevelStarsManager>();
            levelStarsManager.ShowStars(stars);
        }

        public void Play()
        {
            Debug.Log("Play " + _index);
        }
    }
}
