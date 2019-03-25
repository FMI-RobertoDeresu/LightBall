using UnityEngine;

namespace Assets.Scripts.Modules.Stage.GameOver
{
    public class LevelStarsManager : MonoBehaviour
    {
        public GameObject[] stars;

        public void ShowStars(int starsCount)
        {
            for (var i = 0; i < stars.Length; i++)
            {
                var levelStar = stars[i].GetComponent<LevelStar>();
                levelStar.BeforeStart(true, i < starsCount);
            }
        }
    }
}
