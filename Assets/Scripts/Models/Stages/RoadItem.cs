using Assets.Scripts.Models.Stages.Enums;

namespace Assets.Scripts.Models.Stages
{
    public class RoadItem
    {
        public float? Position { get; set; }
        public RoadItemType? Type { get; set; }
        public RoadItemSide? Side { get; set; }
    }
}