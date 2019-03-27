using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.ModuleModels.Stage;
using Assets.Scripts.ServiceModels.ConfigServiceModels.Stages;
using Assets.Scripts.Services;
using Assets.Scripts.Utils;
using PathCreation;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Modules.Stage
{
    public class StageManager : MonoBehaviour
    {
        private VertexPath _roadPath;
        private int _levelIndex;
        private StageInfo _levelInfo;

        private int _pointsPerItem;
        private int _totalPoints;

        private List<RoadItem> _ballsToReach;
        private bool[] _ballsToReachStatus;

        private GameObject _currentMessageGo;
        private GameObject _currentCollisionPointsGo;

        private BallManager _ballManager;
        private Text _messageComponent;

        [Header("Stage objects")]
        public GameObject roadGo;
        public GameObject ballGo;
        public Canvas screenGo;
        public Slider progressBarGo;
        public Text progressBarTextGo;
        public Text totalPointsGo;
        public GameObject[] prefabs;

        private void Awake()
        {
            _levelIndex = AppManager.Instance.AppContext.LevelToPlay;
            _levelInfo = AppManager.Instance.ConfigService.StagesConfig.Stages[_levelIndex];

            _pointsPerItem = 0;
            _totalPoints = 0;

            _ballsToReach = StageUtils.GetBallsToReach(_levelInfo);
            _ballsToReachStatus = new bool[_ballsToReach.Count];

            _roadPath = new VertexPath(new BezierPath(_levelInfo.RoadPointsVector3));

            var roadManager = roadGo.GetComponent<RoadManager>();
            roadManager.RenderRoad(_roadPath, _levelInfo.RoadItems, _levelInfo.Ball.StartOffset.Value);

            _ballManager = ballGo.GetComponent<BallManager>();
            _ballManager.Init(_roadPath, _levelInfo.Ball, OnBallCollision, OnBallOvercome, OnEndPortalReached);
        }

        private void Start()
        {
            AppManager.Instance.SceneLoader.SceneIsReady();
        }

        private void Update()
        {
            // progress and points
            var offset = _levelInfo.Ball.StartOffset.Value;
            var progress = _ballManager.DistanceTraveled / (_roadPath.length - offset);
            progressBarGo.value = progress;
            progressBarTextGo.text = $"{Convert.ToInt16(progress * 100)}%";

            if (_totalPoints > 0)
                totalPointsGo.text = _totalPoints.ToString();
        }

        private void OnBallCollision(GameObject roadItemGo)
        {
            var index = CommonUtils.GetEndingNumber(roadItemGo.name);
            var ballRoadItem = _levelInfo.RoadItems[index];

            if (ballRoadItem.Type == _ballManager.CurrentType)
                OnCorrectBallCollision(roadItemGo, ballRoadItem);
            else
                OnWronBallCollision();
        }

        private void OnBallOvercome(GameObject roadItemGo)
        {
            var index = CommonUtils.GetEndingNumber(roadItemGo.name);
            var ballRoadItem = _levelInfo.RoadItems[index];

            if (ballRoadItem.Type == _ballManager.CurrentType)
                StartCoroutine(ShowMessage("Miss"));
        }

        private void OnCorrectBallCollision(GameObject roadItemGo, RoadItem ballRoadItem)
        {
            var index = _ballsToReach.IndexOf(ballRoadItem);
            _ballsToReachStatus[index] = true;

            var lastMissed = index > 0 && !_ballsToReachStatus[index - 1];
            _pointsPerItem = !lastMissed
                ? Math.Min(_pointsPerItem + _levelInfo.PointsIncrementPerItem.Value, _levelInfo.MaxPointsPerItem.Value)
                : _levelInfo.PointsIncrementPerItem.Value;

            _totalPoints += _pointsPerItem;

            StartCoroutine(ShowOnCollisionPoints(_pointsPerItem.ToString()));
            Destroy(roadItemGo);
        }

        private void OnWronBallCollision()
        {
            ShowGameOver(false);
        }

        private void OnEndPortalReached()
        {
            ShowGameOver(true);
        }

        private IEnumerator ShowMessage(string value)
        {
            if (_currentMessageGo != null)
                Destroy(_currentMessageGo);

            var messagePrefab = prefabs.First(x => x.name == "LevelMessage");
            _currentMessageGo = Instantiate(messagePrefab, screenGo.transform);
            _currentMessageGo.GetComponent<Text>().text = value;

            yield return new WaitForSeconds(1);
            Destroy(_currentMessageGo);
        }

        private IEnumerator ShowOnCollisionPoints(string value)
        {
            var collisionPointsPrefab = prefabs.First(x => x.name == "LevelCollisionPoints");
            var collisionPointsGo = Instantiate(collisionPointsPrefab);
            collisionPointsGo.transform.SetParent(screenGo.transform, false);
            collisionPointsGo.GetComponent<Text>().text = "+" + value;

            _currentCollisionPointsGo = collisionPointsGo;

            var progress = 0f;
            while (progress <= 1f)
            {
                // if another object was created, show only that object
                if (collisionPointsGo != _currentCollisionPointsGo)
                    break;

                progress += 0.05f;
                var position = _ballManager.ballCamera.WorldToScreenPoint(_ballManager.Position);
                position.y += 30 + progress * 25;
                collisionPointsGo.transform.position = position;
                collisionPointsGo.transform.localScale *= 1.0025f;
                yield return new WaitForSeconds(0.01f);
            }

            Destroy(collisionPointsGo);
        }

        private void ShowGameOver(bool finshed)
        {
            var playerData = AppManager.Instance.PlayerData.Data;
            var missedBalls = _ballsToReachStatus.Count(x => !x);
            var stars = finshed ? Mathf.Max(1, 3 - missedBalls) : 0;
            var bestScore = Math.Max(playerData.LevelsBestScore[_levelIndex], _totalPoints);

            var gameOverInfo = new GameOverScreenInfo
            {
                LevelName = _levelInfo.Name,
                Stars = stars,
                Score = _totalPoints,
                BestScore = bestScore
            };

            AppManager.Instance.AppContext.GameOverInfo = gameOverInfo;
            StartCoroutine(AppManager.Instance.SceneLoader.LoadScene(SceneNames.GameOver, false));

            playerData.MaxPlayedLevel = Math.Max(playerData.MaxPlayedLevel, _levelIndex);
            playerData.LevelsBestScore[_levelIndex] = bestScore;
            playerData.LevelsStars[_levelIndex] = stars;
            AppManager.Instance.PlayerData.SaveChanges();
        }
    }
}