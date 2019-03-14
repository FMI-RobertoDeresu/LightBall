using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Models.Stages
{
    [Serializable]
    public class Stage
    {
        public float speed;
        public RoadPoint[] roadPoints;
        public RoadItem[] roadItems;

        public Vector3[] RoadPointsVector3 => roadPoints.Select(x => x.ToVector3).ToArray();
    }
}