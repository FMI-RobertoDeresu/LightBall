using System.Linq;
using Assets.Scripts.ServiceModels.ConfigServiceModels.Stages.Enums;
using UnityEngine;

namespace Assets.Scripts.Modules.Stage
{
    public class BallCollision : MonoBehaviour
    {
        private BallManager _ballManager;

        private void Awake()
        {
            _ballManager = GetComponent<BallManager>();
        }

        private void OnCollisionEnter(Collision col)
        {
            var isSphereCollider = col.contacts[0].thisCollider is SphereCollider;
            if (isSphereCollider)
            {
                var objectIsBall = RoadItems.Balls.Any(x => x.ToString() == col.gameObject.tag);
                if (objectIsBall)
                    _ballManager.OnBallCollision(col.gameObject);

                var objectIsSwitch = RoadItems.Switches.Any(x => x.ToString() == col.gameObject.tag);
                if (objectIsSwitch)
                    _ballManager.OnSwitchCollision(col.gameObject);

                var objectIsPortal = col.gameObject.tag == RoadItemType.Portal.ToString();
                if (objectIsPortal)
                    _ballManager.OnPortalCollision(col.gameObject);
            }
        }

        private void OnCollisionExit(Collision col)
        {
            var objectIsBall = RoadItems.Balls.Any(x => x.ToString() == col.gameObject.tag);
            if (objectIsBall)
                _ballManager.OnBallOvercome(col.gameObject);
        }
    }
}