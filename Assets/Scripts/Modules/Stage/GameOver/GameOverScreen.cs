using Assets.Scripts.Services;
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

        private void Awake()
        {
            _levelText = levelGo.GetComponent<Text>();
            _starsManager = starsGo.GetComponent<LevelStarsManager>();
            _scoreText = scoreGo.GetComponent<Text>();
            _bestScoreText = bestScoreGo.GetComponent<Text>();
        }

        public void Start()
        {
            var info = AppManager.Instance.AppContext.GameOverInfo;
            _levelText.text = info.LevelName;
            _starsManager.ShowStars(info.Stars);
            _scoreText.text = info.Score.ToString();
            _bestScoreText.text = info.BestScore > 0 ? $"Best score: {info.BestScore.ToString()}" : string.Empty;
        }
    }
}