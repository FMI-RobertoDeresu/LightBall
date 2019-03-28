using System;
using Assets.Scripts.ServiceModels.ConfigServiceModels.Levels.Enums;
using Assets.Scripts.Utils;

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
                case RoadItemType.RedSwitch:
                case RoadItemType.BlueSwitch:
                case RoadItemType.YellowSwitch:
                case RoadItemType.PurpleSwitch:
                    materialName = $"Switch {roadItemType.ToString().Replace("Switch", "")}";
                    break;
                case RoadItemType.Portal:
                    materialName = null;
                    break;
                default:
                    throw new NotSupportedException();
            }

            return materialName;
        }

        public static string GetDisolveMaterialName(this RoadItemType roadItemType)
        {
            var materialName = roadItemType.GetMaterialName();
            var disolveMaterialName = $"{materialName} Disolve";

            return disolveMaterialName;
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
                case RoadItemType.RedSwitch:
                case RoadItemType.BlueSwitch:
                case RoadItemType.YellowSwitch:
                case RoadItemType.PurpleSwitch:
                    prefabName = "ColorSwitch";
                    break;
                case RoadItemType.Portal:
                    prefabName = "Portal";
                    break;
                default:
                    throw new NotSupportedException();
            }

            return prefabName;
        }

        public static RoadItemType GetSwitchBallType(this RoadItemType roadItemType)
        {
            RoadItemType type;
            switch (roadItemType)
            {
                case RoadItemType.RedSwitch:
                case RoadItemType.BlueSwitch:
                case RoadItemType.YellowSwitch:
                case RoadItemType.PurpleSwitch:
                    var typeStr = $"{roadItemType.ToString().Replace("Switch", "")}Ball";
                    type = EnumUtils.Parse<RoadItemType>(typeStr);
                    break;
                default:
                    throw new NotSupportedException();
            }

            return type;
        }
    }
}