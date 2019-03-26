using Assets.Scripts.Models.Config.Stages.Enums;

namespace Assets.Scripts.Models.Config.Stages
{
    public class BallConfig
    {
        public float? Speed { get; set; }
        public RoadItemType? InitialType { get; set; }
        public float? StartOffset { get; set; }
    }
}