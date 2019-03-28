namespace Assets.Scripts.ServiceModels.ConfigServiceModels.Levels
{
    public class LevelInfo
    {
        public string Name { get; set; }

        public int? PointsIncrementPerItem { get; set; }
        public int? MaxPointsPerItem { get; set; }

        public BallConfig Ball { get; set; }
        public RoadConfig Road { get; set; }
    }
}