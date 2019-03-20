using UnityEngine;

namespace Assets.Scripts
{
    public class BallCollision : MonoBehaviour
    {
        private void OnCollisionEnter(Collision col)
        {
            var ballManager = GetComponent<BallManager>();
            if (col.gameObject.name.Contains("_Ball_"))
                ballManager.OnRoadItemBallCollision(col.gameObject);

            if (col.gameObject.name.Contains("RoadItem_Portal"))
                ballManager.OnPortalCollision(col.gameObject);
        }
    }
}