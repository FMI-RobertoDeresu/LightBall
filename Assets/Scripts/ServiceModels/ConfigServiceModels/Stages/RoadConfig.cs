using System.Linq;
using UnityEngine;

namespace Assets.Scripts.ServiceModels.ConfigServiceModels.Stages
{
    public class RoadConfig
    {
        public float? StartOffset { get; set; }
        public float? EndOffset { get; set; }

        public RoadPoint[] Points { get; set; }
        public RoadItem[] Items { get; set; }

        public Vector3[] PointsVector3 => Points.Select(x => x.ToVector3).ToArray();
    }
}