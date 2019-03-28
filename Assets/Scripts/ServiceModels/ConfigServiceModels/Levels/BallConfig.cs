using Assets.Scripts.ServiceModels.ConfigServiceModels.Levels.Enums;

namespace Assets.Scripts.ServiceModels.ConfigServiceModels.Levels
{
    public class BallConfig
    {
        public float? Speed { get; set; }
        public RoadItemType? InitialType { get; set; }
        public float? StartOffset { get; set; }
    }
}