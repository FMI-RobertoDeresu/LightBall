using System.Linq;
using Assets.Scripts.ServiceModels.ConfigServiceModels.Stages.Enums;
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
            var objectIsBall = RoadItems.Balls.Any(x => x.ToString() == col.gameObject.tag);
            if (objectIsBall)
                _ballManager.OnRoadItemBallCollision(col.gameObject);

            var objectIsSwitch = RoadItems.Switches.Any(x => x.ToString() == col.gameObject.tag);
            if (objectIsSwitch)
                _ballManager.OnRoadItemSwitchCollision(col.gameObject);

            var objectIsPortal = col.gameObject.tag == RoadItemType.Portal.ToString();
            if (objectIsPortal)
                _ballManager.OnPortalCollision(col.gameObject);
        }
    }
}