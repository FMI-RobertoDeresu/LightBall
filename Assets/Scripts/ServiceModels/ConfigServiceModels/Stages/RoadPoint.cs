using UnityEngine;

namespace Assets.Scripts.ServiceModels.ConfigServiceModels.Stages
{
    public class RoadPoint
    {
        public float? X { get; set; }
        public float? Y { get; set; }
        public float? Z { get; set; }

        public Vector3 ToVector3 => new Vector3(X.Value, Y.Value, Z.Value);
    }
}