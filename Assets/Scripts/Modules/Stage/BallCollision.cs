using System.Linq;
using Assets.Scripts.Models.Config.Stages.Enums;
using UnityEngine;

namespace Assets.Scripts.Modules.Stage
{
    public class BallCollision : MonoBehaviour
    {
        private BallManager _ballManager;

        private void Start()
        {
            _ballManager = GetComponent<BallManager>();
        }

        private void OnCollisionEnter(Collision col)
        {

            var balls = new[]
            {
                RoadItemType.RedBall,
                RoadItemType.BlueBall,
                RoadItemType.YellowBall,
                RoadItemType.PurpleBall
            };
            var objectIsBall = balls.Any(x => x.ToString() == col.gameObject.tag);
            if (objectIsBall)
                _ballManager.OnRoadItemBallCollision(col.gameObject);

            var switches = new[]
            {
                RoadItemType.RedSwitch,
                RoadItemType.BlueSwitch,
                RoadItemType.YellowSwitch,
                RoadItemType.PurpleSwitch
            };
            var objectIsSwitch = switches.Any(x => x.ToString() == col.gameObject.tag);
            if (objectIsSwitch)
                _ballManager.OnRoadItemSwitchCollision(col.gameObject);

            var objectIsPortal = col.gameObject.tag == RoadItemType.Portal.ToString();
            if (objectIsPortal)
                _ballManager.OnPortalCollision(col.gameObject);
        }
    }
}