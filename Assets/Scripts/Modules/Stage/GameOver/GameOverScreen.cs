using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Modules.Stage.GameOver
{
    public class GameOverScreen : MonoBehaviour
    {
        private Text _levelText;
        private LevelStarsManager _starsManager;
        private Text _scoreText;
        private Text _bestScoreText;

        public GameObject levelGo;
        public GameObject starsGo;
        public GameObject scoreGo;
        public GameObject bestScoreGo;

        void Start()
        {
            _levelText = levelGo.GetComponent<Text>();
            _starsManager = starsGo.GetComponent<LevelStarsManager>();
            _scoreText = scoreGo.GetComponent<Text>();
            _bestScoreText = bestScoreGo.GetComponent<Text>();
        }

        public void Show(string levelTxt, int starsCount, int score, int bestScore)
        {
            _levelText.text = levelTxt;
            _starsManager.ShowStars(starsCount);
            _scoreText.text = score.ToString();
            _bestScoreText.text = $"Best score: {bestScore.ToString()}";
        }
    }
}
