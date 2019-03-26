using Assets.Scripts.Models.Config.Stages.Enums;

namespace Assets.Scripts.Models.Config.Stages
{
    public class RoadItem
    {
        public float? Position { get; set; }
        public RoadItemType? Type { get; set; }
        public RoadItemSide? Side { get; set; }
    }
}