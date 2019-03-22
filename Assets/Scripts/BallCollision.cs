using System.Linq;
using Assets.Scripts.Models.Stages.Enums;
using UnityEngine;

namespace Assets.Scripts
{
    public class BallCollision : MonoBehaviour
    {
        private void OnCollisionEnter(Collision col)
        {
            var ballManager = GetComponent<BallManager>();
            var balls = new[]
            {
                RoadItemType.BlueBall,
                RoadItemType.PurpleBall,
                RoadItemType.RedBall,
                RoadItemType.YellowBall
            };
            var objectIsBall = balls.Any(x => x.ToString() == col.gameObject.tag);
            if (objectIsBall)
                ballManager.OnRoadItemBallCollision(col.gameObject);

            var objectIsPortal = col.gameObject.tag == RoadItemType.Portal.ToString();
            if (objectIsPortal)
                ballManager.OnPortalCollision(col.gameObject);
        }
    }
}