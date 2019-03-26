using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Models.Config.Stages;
using Assets.Scripts.Utils;
using PathCreation;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Modules.Stage
{
    public class StageManager : MonoBehaviour
    {
        private bool _ready;
        private float _percentTraveled;
        private VertexPath _roadPath;
        private Models.Config.Stages.Stage _stageInfo;
        private List<RoadItem> _reachedRoadItems;
        private GameObject _currentCollisionPointsGo;

        private int _pointsPerItem;
        private int _totalPoints;

        private BallManager _ballManager;

        [Header("Stage objects")]
        public GameObject roadGo;
        public GameObject ballGo;
        public Canvas screenGo;
        public Slider progressBarGo;
        public Text progressBarTextGo;
        public Text totalPointsGo;
        public GameObject[] prefabs;

        private void Start()
        {
            // resolve stage
            RenderStage(null);
        }

        private void Update()
        {
            if (!_ready)
                return;

            if (_ballManager.DistanceTraveled > 0)
            {
                var offset = _stageInfo.Ball.StartOffset.Value;
                _percentTraveled = (_ballManager.DistanceTraveled) / (_roadPath.length - offset);
                progressBarGo.value = _percentTraveled;
                progressBarTextGo.text = $"{Convert.ToInt16(_percentTraveled * 100)}%";
            }

            if (_totalPoints > 0)
                totalPointsGo.text = _totalPoints.ToString();
        }

        private void OnBallCollision(GameObject roadItemGo)
        {
            var index = CommonUtils.GetEndingNumber(roadItemGo.name);
            var ballRoadItem = _stageInfo.RoadItems[index];
            var firstOfType = _stageInfo.RoadItems.FirstOrDefault(x =>
                !_reachedRoadItems.Contains(x) && x.Type == _ballManager.CurrentType);
            var anyMissed = firstOfType != null && firstOfType.Position < ballRoadItem.Position;

            _pointsPerItem = !anyMissed
                ? Math.Min(_pointsPerItem + _stageInfo.PointsIncrementPerItem.Value, _stageInfo.MaxPointsPerItem.Value)
                : _stageInfo.PointsIncrementPerItem.Value;

            _totalPoints += _pointsPerItem;
            StartCoroutine(ShowOnCollisionPoints(_pointsPerItem.ToString()));

            _reachedRoadItems.Add(ballRoadItem);
            Destroy(roadItemGo);
        }

        private IEnumerator ShowOnCollisionPoints(string value)
        {
            var collisionPointsPrefab = prefabs.First(x => x.name == "CollisionPoints");
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

        private void OnEndPortalReached()
        {
            SceneManager.LoadScene(SceneNames.GameOver, LoadSceneMode.Single);
            SceneManager.sceneLoaded += (scene, mode) =>
            {
                scene.isDirty 
            };
            // params to gameover scene
        }

        public void RenderStage(Models.Config.Stages.Stage stage)
        {
            _pointsPerItem = 0;
            _totalPoints = 0;
            _reachedRoadItems = new List<RoadItem>();

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
    }
}