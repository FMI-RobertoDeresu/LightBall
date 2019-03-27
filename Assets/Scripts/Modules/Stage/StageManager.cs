using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Extensions;
using Assets.Scripts.ModuleModels.Stage;
using Assets.Scripts.ServiceModels.ConfigServiceModels.Stages;
using Assets.Scripts.ServiceModels.ConfigServiceModels.Stages.Enums;
using Assets.Scripts.Services;
using Assets.Scripts.Utils;
using PathCreation;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Modules.Stage
{
    public class StageManager : MonoBehaviour
    {
        private bool _ready;
        private VertexPath _roadPath;
        private int _levelIndex;
        private StageInfo _levelInfo;

        private List<RoadItem> _ballToReach;
        private bool[] _ballToReachStatus;

        private GameObject _currentMessageGo;
        private GameObject _currentCollisionPointsGo;

        private int _pointsPerItem;
        private int _totalPoints;
        private int _missedBalls;

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
        }

        private void Start()
        {
            RenderStage(_levelInfo);
            AppManager.Instance.SceneLoader.SceneIsReady();
        }

        private void Update()
        {
            if (!_ready)
                return;

            // progress and points
            var offset = _levelInfo.Ball.StartOffset.Value;
            var percentTraveled = (_ballManager.DistanceTraveled) / (_roadPath.length - offset);
            progressBarGo.value = percentTraveled;
            progressBarTextGo.text = $"{Convert.ToInt16(percentTraveled * 100)}%";

            if (_totalPoints > 0)
                totalPointsGo.text = _totalPoints.ToString();

            //// check for missed balls
            //var firstOfType = _levelInfo.RoadItems.FirstOrDefault(x =>
            //    !_roadItemsReached.Contains(x) && x.Type == _ballManager.CurrentType);
            //var anyMissed = firstOfType != null && firstOfType.Position < ballRoadItem.Position;

            //_nextBallToReach  = 
        }

        public void RenderStage(StageInfo stage)
        {
            _pointsPerItem = 0;
            _totalPoints = 0;
            _missedBalls = 0;
            _ballToReach = new List<RoadItem>();

            var roadItemsChunks = stage.RoadItems.Split(x => RoadItems.Switches.Contains(x.Type.Value)).ToList();
            foreach (var chunk in roadItemsChunks)
            {
                var separator = stage.RoadItems.LastOrDefault(x => RoadItems.Switches.Contains(x.Type.Value) && x.Position < chunk.First().Position);
                var afterSwitchBallType = separator?.Type.Value.GetSwitchBallType() ?? stage.Ball.InitialType.Value;
                _ballToReach.AddRange(chunk.Where(x=>x.Type == afterSwitchBallType));
            }

            _ballToReachStatus = new bool[_ballToReach.Count];


            _roadPath = new VertexPath(new BezierPath(stage.RoadPointsVector3));

            var roadManager = roadGo.GetComponent<RoadManager>();
            roadManager.RenderRoad(_roadPath, stage.RoadItems);

            _ballManager = ballGo.GetComponent<BallManager>();
            _ballManager.BeforeStart(
                _roadPath,
                stage.Ball.Speed.Value,
                stage.Ball.InitialType.Value,
                stage.Ball.StartOffset.Value,
                OnBallCollision,
                OnEndPortalReached);

            _ready = true;
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

        private void OnCorrectBallCollision(GameObject roadItemGo, RoadItem ballRoadItem)
        {
            var index = _ballToReach.IndexOf(ballRoadItem);
            _ballToReachStatus[index] = true;

            var lastMissed = index > 0 && !_ballToReachStatus[index - 1];
            _pointsPerItem = !lastMissed
                ? Math.Min(_pointsPerItem + _levelInfo.PointsIncrementPerItem.Value, _levelInfo.MaxPointsPerItem.Value)
                : _levelInfo.PointsIncrementPerItem.Value;

            _totalPoints += _pointsPerItem;

            StartCoroutine(ShowOnCollisionPoints(_pointsPerItem.ToString()));
            Destroy(roadItemGo);
        }

        private void OnWronBallCollision()
        {
            StartCoroutine(ShowMessage("WRONG"));
        }

        private void OnEndPortalReached()
        {
            var playerData = AppManager.Instance.PlayerData.Data;
            var stars = Mathf.Max(1, 3 - _missedBalls);
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


    }
}