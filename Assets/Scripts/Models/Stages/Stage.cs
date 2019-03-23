using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Models.Stages
{
    public class Stage
    {
        public int? PointsIncrementPerItem { get; set; }
        public int? MaxPointsPerItem { get; set; }

        public BallConfig Ball { get; set; }

        public RoadPoint[] RoadPoints { get; set; }
        public RoadItem[] RoadItems { get; set; }

        public Vector3[] RoadPointsVector3 => RoadPoints.Select(x => x.ToVector3).ToArray();
    }
}