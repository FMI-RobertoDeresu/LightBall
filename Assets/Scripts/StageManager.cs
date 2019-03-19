using System;
using System.IO;
using Assets.Scripts.Models.Stages;
using Newtonsoft.Json;
using PathCreation;
using UnityEngine;

namespace Assets.Scripts
{
    public class StageManager : MonoBehaviour
    {
        [Header("Stage objects")]
        public GameObject road;
        public GameObject ball;

        private void Start()
        {
            Test();
        }

        public void RenderStage(Stage stage)
        {
            var roadPath = new VertexPath(new BezierPath(stage.RoadPointsVector3));

            var roadManager = road.GetComponent<RoadManager>();
            roadManager.RenderRoad(roadPath, stage.RoadItems);

            var ballManager = ball.GetComponent<BallManager>();
            ballManager.BeforeStart(roadPath, stage.Speed.Value);
        }

        private void Test()
        {
            try
            {
                var stagesPath = Path.Combine(Application.dataPath, "Config/Stages.json");
                var stagesFileContent = File.ReadAllText(stagesPath);
                var stagesConfig = JsonConvert.DeserializeObject<StagesConfig>(stagesFileContent);
                var stage = stagesConfig.Stages[0];

                RenderStage(stage);
            }
            catch (Exception exception)
            {
                Debug.Log(exception);
            }
        }
    }
}