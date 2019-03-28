using System.Collections.Generic;

namespace Assets.Scripts.ServiceModels.ConfigServiceModels.Levels.Enums
{
    public class RoadItems
    {
        public static List<RoadItemType> Balls =>
            new List<RoadItemType> { RoadItemType.RedBall, RoadItemType.BlueBall, RoadItemType.YellowBall, RoadItemType.PurpleBall };

        public static List<RoadItemType> Switches =>
            new List<RoadItemType> { RoadItemType.RedSwitch, RoadItemType.BlueSwitch, RoadItemType.YellowSwitch, RoadItemType.PurpleSwitch };
    }

    public enum RoadItemType
    {
        RedBall,
        BlueBall,
        YellowBall,
        PurpleBall,

        RedSwitch,
        BlueSwitch,
        YellowSwitch,
        PurpleSwitch,

        Magnet,
        Multiplier,
        SuperBall,

        Coin,
        Diamond,

        Portal
    }
}