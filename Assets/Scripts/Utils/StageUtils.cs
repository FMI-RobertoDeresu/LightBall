using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Extensions;
using Assets.Scripts.ServiceModels.ConfigServiceModels.Stages;
using Assets.Scripts.ServiceModels.ConfigServiceModels.Stages.Enums;

namespace Assets.Scripts.Utils
{
    public class StageUtils
    {
        public static List<RoadItem> GetBallsToReach(StageInfo stage)
        {
            var ballToReach = new List<RoadItem>();

            var roadItemsChunks = stage.RoadItems.Split(x => RoadItems.Switches.Contains(x.Type.Value)).ToList();
            foreach (var chunk in roadItemsChunks)
            {
                var separator = stage.RoadItems.LastOrDefault(x => RoadItems.Switches.Contains(x.Type.Value) && x.Position < chunk.First().Position);
                var afterSwitchBallType = separator?.Type.Value.GetSwitchBallType() ?? stage.Ball.InitialType.Value;
                ballToReach.AddRange(chunk.Where(x => x.Type == afterSwitchBallType));
            }

            return ballToReach;
        }
    }
}