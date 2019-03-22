using System.Linq;
using Assets.Scripts.Models.Stages.Enums;
using UnityEngine;

namespace Assets.Scripts.Models.Stages
{
    public class Stage
    {
        public int? PointsIncrementPerItem { get; set; }
        public int? MaxPointsPerItem { get; set; }

        public float? Speed { get; set; }
        public RoadItemType? InitialType { get; set; }

        public RoadPoint[] RoadPoints { get; set; }
        public RoadItem[] RoadItems { get; set; }

        public Vector3[] RoadPointsVector3 => RoadPoints.Select(x => x.ToVector3).ToArray();
    }
}