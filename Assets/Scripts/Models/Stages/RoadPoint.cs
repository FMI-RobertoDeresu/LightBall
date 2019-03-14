using System;
using UnityEngine;

namespace Assets.Scripts.Models.Stages
{
    [Serializable]
    public class RoadPoint
    {
        public float x;
        public float y;
        public float z;

        public Vector3 ToVector3 => new Vector3(x, y, z);
    }
}