using Assets.Scripts.Models.Stages.Enums;

namespace Assets.Scripts.Models.Stages
{
    public class BallConfig
    {
        public float? Speed { get; set; }
        public RoadItemType? InitialType { get; set; }
        public float? StartOffset { get; set; }
    }
}