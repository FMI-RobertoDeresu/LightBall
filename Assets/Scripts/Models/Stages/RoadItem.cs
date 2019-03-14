using System;
using Assets.Scripts.Models.Stages.Enums;

namespace Assets.Scripts.Models.Stages
{
    [Serializable]
    public class RoadItem
    {
        public float position;
        public RoadItemType type;
        public RoadItemSide side;
    }
}