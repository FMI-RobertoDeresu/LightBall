using Assets.Scripts.ServiceModels.ConfigServiceModels.Stages.Enums;

namespace Assets.Scripts.ServiceModels.ConfigServiceModels.Stages
{
    public class RoadItem
    {
        public float? Position { get; set; }
        public RoadItemType? Type { get; set; }
        public RoadItemSide? Side { get; set; }
    }
}