using System.Linq;
using Assets.Scripts.ServiceModels.ConfigServiceModels.Levels.Enums;
using UnityEngine;

namespace Assets.Scripts.Modules.Level
{
    public class BallCollision : MonoBehaviour
    {
        private int _rayCastLayerMask;
        private float _rayCastMaxDistance;
        private GameObject _nextColorSwitch;
        private BallManager _ballManager;

        private void Awake()
        {
            _rayCastLayerMask = 1 << 9;
            _rayCastMaxDistance = 3f;
            _ballManager = GetComponent<BallManager>();
        }

        private void Update()
        {
            CheckForColorSwitches();
        }

        private void CheckForColorSwitches()
        {
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out var hit,
                _rayCastMaxDistance, _rayCastLayerMask))
            {
                _nextColorSwitch = hit.collider.gameObject;
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance,
                    Color.yellow, 1, false);
            }
            else if (_nextColorSwitch != null)
            {
                _ballManager.OnSwitchCollision(_nextColorSwitch);
                _nextColorSwitch = null;
            }
        }

        private void OnCollisionEnter(Collision col)
        {
            var isSphereCollider = col.contacts[0].thisCollider is SphereCollider;
            if (isSphereCollider)
            {
                var objectIsBall = RoadItems.Balls.Any(x => x.ToString() == col.gameObject.tag);
                if (objectIsBall)
                    _ballManager.OnBallCollision(col.gameObject);

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