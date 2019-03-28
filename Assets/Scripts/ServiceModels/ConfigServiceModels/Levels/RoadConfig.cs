using System.Linq;
using UnityEngine;

namespace Assets.Scripts.ServiceModels.ConfigServiceModels.Levels
{
    public class RoadConfig
    {
        public float? ItemsStartOffset { get; set; }
        public float? ItemsEndOffset { get; set; }

        public RoadPoint[] Points { get; set; }
        public RoadItem[] Items { get; set; }

        public Vector3[] PointsVector3 => Points.Select(x => x.ToVector3).ToArray();
    }
}