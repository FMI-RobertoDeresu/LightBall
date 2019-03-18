using Assets.Scripts.Models.Stages;
using PathCreation;
using UnityEngine;

namespace Assets.Scripts
{
    public class RoadItemsManager : MonoBehaviour
    {
        [Header("Settings")]
        public GameObject[] prefabs;
        public Material[] materials;

        public void RenderItems(VertexPath roadPath, RoadItem[] roadItems)
        {
            for (var i = 0; i < roadItems.Length; i++)
            {
                var roadItem = roadItems[i];
                var position = roadPath.GetPoint(roadItem.position);

                var ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                ball.name = "RoadItem_ " + i;
                ball.transform.position = position;
                ball.transform.Translate(Vector3.up * 0.5f);

                var rigidBody = ball.AddComponent<Rigidbody>();
                rigidBody.isKinematic = true;

                var meshRenderer = ball.GetComponent<MeshRenderer>();
                //meshRenderer.material = GameObject.ma
            }
        }
    }
}