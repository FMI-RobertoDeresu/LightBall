using Assets.Scripts.ServiceModels.ConfigServiceModels.Levels.Enums;

namespace Assets.Scripts.ServiceModels.ConfigServiceModels.Levels
{
    public class RoadItem
    {
        public float? Position { get; set; }
        public RoadItemType? Type { get; set; }
        public RoadItemSide? Side { get; set; }
    }
}