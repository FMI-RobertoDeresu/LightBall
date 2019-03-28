using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Modules.Stage.GameOver
{
    public class LevelStarsManager : MonoBehaviour
    {
        private IList<LevelStar> _stars;

        public GameObject[] starsGo;

        private void Awake()
        {
            _stars = starsGo.Select(x => x.GetComponent<LevelStar>()).ToList();
        }

        public void ShowStars(int starsCount)
        {
            for (var i = 0; i < _stars.Count; i++)
                _stars[i].Display(true, i < starsCount);
        }
    }
}
