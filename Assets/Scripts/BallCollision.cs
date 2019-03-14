using UnityEngine;

namespace Assets.Scripts
{
    public class BallCollision : MonoBehaviour
    {
        private void OnCollisionEnter(Collision col)
        {
            if (col.gameObject.name == "prop_powerCube")
                Destroy(col.gameObject);
            Debug.Log(col.gameObject.name);
        }
    }
}