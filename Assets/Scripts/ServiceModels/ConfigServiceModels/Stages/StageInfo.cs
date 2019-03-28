using System.Linq;
using UnityEngine;

namespace Assets.Scripts.ServiceModels.ConfigServiceModels.Stages
{
    public class StageInfo
    {
        public string Name { get; set; }

        public int? PointsIncrementPerItem { get; set; }
        public int? MaxPointsPerItem { get; set; }

        public BallConfig Ball { get; set; }
        public RoadConfig Road { get; set; }
    }
}