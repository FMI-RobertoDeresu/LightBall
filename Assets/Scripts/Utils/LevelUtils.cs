using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Extensions;
using Assets.Scripts.ServiceModels.ConfigServiceModels.Levels;
using Assets.Scripts.ServiceModels.ConfigServiceModels.Levels.Enums;

namespace Assets.Scripts.Utils
{
    public class LevelUtils
    {
        public static List<RoadItem> GetBallsToReach(LevelInfo level)
        {
            var ballToReach = new List<RoadItem>();

            var roadItemsChunks = level.Road.Items.Split(x => RoadItems.Switches.Contains(x.Type.Value)).ToList();
            foreach (var chunk in roadItemsChunks)
            {
                var separator = level.Road.Items.LastOrDefault(x => RoadItems.Switches.Contains(x.Type.Value) && x.Position < chunk.First().Position);
                var afterSwitchBallType = separator?.Type.Value.GetSwitchBallType() ?? level.Ball.InitialType.Value;
                ballToReach.AddRange(chunk.Where(x => x.Type == afterSwitchBallType));
            }

            return ballToReach;
        }
    }
}