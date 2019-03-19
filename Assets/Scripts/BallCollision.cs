using UnityEngine;

namespace Assets.Scripts
{
    public class BallCollision : MonoBehaviour
    {
        private void OnCollisionEnter(Collision col)
        {
            if (col.gameObject.name.StartsWith("RoadItem"))
                Destroy(col.gameObject);
        }
    }
}