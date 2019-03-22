using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Scripts.Models.Stages;
using Assets.Scripts.Utils;
using Newtonsoft.Json;
using PathCreation;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class StageManager : MonoBehaviour
    {
        private bool _ready;
        private float _percentTraveled;
        private VertexPath _roadPath;
        private Stage _stageInfo;
        private List<RoadItem> _reachedRoadItems;

        private int _pointsPerItem;
        private int _totalPoints;

        private BallManager _ballManager;

        [Header("Stage objects")]
        public GameObject roadGo;
        public GameObject ballGo;
        public Slider progressBarGo;
        public Text totalPointsGo;
        public Text onCollisionPointsGo;

        private void Start()
        {
            Test();
        }

        private void Update()
        {
            if (!_ready)
                return;

            if (_ballManager.DistanceTraveled > 0)
            {
                _percentTraveled = _ballManager.DistanceTraveled / _roadPath.length;
                progressBarGo.value = _percentTraveled;
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
            onCollisionPointsGo.text = _pointsPerItem.ToString();

            _reachedRoadItems.Add(ballRoadItem);
        }

        private void OnEndPortalReached()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
            Test();
        }

        public void RenderStage(Stage stage)
        {
            _reachedRoadItems = new List<RoadItem>();
            _pointsPerItem = 0;
            _totalPoints = 0;

            _roadPath = new VertexPath(new BezierPath(stage.RoadPointsVector3));

            var roadManager = roadGo.GetComponent<RoadManager>();
            roadManager.RenderRoad(_roadPath, stage.RoadItems);

            _ballManager = ballGo.GetComponent<BallManager>();
            _ballManager.BeforeStart(
                _roadPath,
                stage.Speed.Value,
                stage.InitialType.Value,
                OnBallCollision,
                OnEndPortalReached);

            _ready = true;
        }


        private void Test()
        {
            try
            {
                var stagesPath = Path.Combine(Application.dataPath, "Config/Stages.json");
                var stagesFileContent = File.ReadAllText(stagesPath);
                var stagesConfig = JsonConvert.DeserializeObject<StagesConfig>(stagesFileContent);
                _stageInfo = stagesConfig.Stages[0];

                RenderStage(_stageInfo);
            }
            catch (Exception exception)
            {
                Debug.Log(exception);
            }
        }
    }
}