using System;
using System.IO;
using Assets.Scripts.Models.Stages;
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
            roadManager.RenderRoad(roadPath, stage.roadItems);

            var ballManager = ball.GetComponent<BallManager>();
            ballManager.BeforeStart(roadPath);
        }

        private void Test()
        {
            try
            {
                var stagesPath = Path.Combine(Application.dataPath, "Config/Stages.json");
                var stagesFileContent = File.ReadAllText(stagesPath);
                var stagesConfig = JsonUtility.FromJson<StagesConfig>(stagesFileContent);
                var stage = stagesConfig.stages[0];

                RenderStage(stage);
            }
            catch (Exception exception)
            {
                Debug.Log(exception);
            }
        }
    }
}