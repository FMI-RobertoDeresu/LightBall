using System;
using System.Collections;
using System.IO;
using Assets.Scripts.Models.Stages;
using Newtonsoft.Json;
using PathCreation;
using UnityEngine;

namespace Assets.Scripts
{
    public class StageManager : MonoBehaviour
    {
        private bool _ready;
        private Vector3[] _roadPoints;
        private RoadItem[] _roadItems;
        private Action _onReadyFn;

        [Header("Stage objects")]
        public GameObject road;
        public GameObject ball;

        public void BeforeStart(Vector3[] roadPoints, RoadItem[] roadItems, Action onReadyFn)
        {
            _roadPoints = roadPoints;
            _roadItems = roadItems;
            _onReadyFn = onReadyFn;
            _ready = true;
        }

        private IEnumerator Start()
        {
            Test();

            Debug.Log($"Waiting for princess {GetType().Name}  to be rescued...");
            yield return new WaitUntil(() => _ready);
            Debug.Log($"Princess {GetType().Name}  was rescued!");

            var roadPath = new VertexPath(new BezierPath(_roadPoints));

            var roadManager = road.GetComponent<RoadManager>();
            roadManager.BeforeStart(roadPath, _roadItems);

            var ballManager = ball.GetComponent<BallManager>();
            ballManager.BeforeStart(roadPath);

            _onReadyFn?.Invoke();
        }

        private void Test()
        {
            try
            {
                var stagesPath = Path.Combine(Application.dataPath, "Config/Stages.json");
                var stagesFileContent = File.ReadAllText(stagesPath);
                var stagesConfig = JsonConvert.DeserializeObject<StagesConfig>(stagesFileContent);
                var stage = stagesConfig.Stages[0];

                BeforeStart(stage.RoadPointsVector3, stage.RoadItems, null);
            }
            catch (Exception exception)
            {
                Debug.Log(exception);
            }
        }
    }
}