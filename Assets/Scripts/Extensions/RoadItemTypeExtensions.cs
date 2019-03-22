using System;
using Assets.Scripts.Models.Stages.Enums;

namespace Assets.Scripts.Extensions
{
    public static class RoadItemTypeExtensions
    {
        public static string GetMaterialName(this RoadItemType roadItemType)
        {
            string materialName;
            switch (roadItemType)
            {
                case RoadItemType.RedBall:
                case RoadItemType.BlueBall:
                case RoadItemType.YellowBall:
                case RoadItemType.PurpleBall:
                    materialName = $"Ball {roadItemType.ToString().Replace("Ball", "")}";
                    break;
                case RoadItemType.RedBallSwitch:
                case RoadItemType.BlueBallSwitch:
                case RoadItemType.YellowBallSwitch:
                case RoadItemType.PurpleBallSwitch:
                case RoadItemType.Magnet:
                case RoadItemType.Multiplier:
                case RoadItemType.SuperBall:
                case RoadItemType.Coin:
                case RoadItemType.Diamond:
                    throw new NotSupportedException();
                case RoadItemType.Portal:
                    materialName = null;
                    break;
                default:
                    throw new NotSupportedException();
            }

            return materialName;
        }

        public static string GetPrefabName(this RoadItemType roadItemType)
        {
            string prefabName;
            switch (roadItemType)
            {
                case RoadItemType.RedBall:
                case RoadItemType.BlueBall:
                case RoadItemType.YellowBall:
                case RoadItemType.PurpleBall:
                    prefabName = "Sphere";
                    break;
                case RoadItemType.RedBallSwitch:
                case RoadItemType.BlueBallSwitch:
                case RoadItemType.YellowBallSwitch:
                case RoadItemType.PurpleBallSwitch:
                case RoadItemType.Magnet:
                case RoadItemType.Multiplier:
                case RoadItemType.SuperBall:
                case RoadItemType.Coin:
                case RoadItemType.Diamond:
                    throw new NotSupportedException();
                case RoadItemType.Portal:
                    prefabName = "Portal";
                    break;
                default:
                    throw new NotSupportedException();
            }

            return prefabName;
        }
    }
}