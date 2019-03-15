using System.Collections;
using Assets.Scripts.Models.Stages;
using PathCreation;
using UnityEngine;

namespace Assets.Scripts
{
    public class RoadItemsManager : MonoBehaviour
    {
        private bool _ready;
        private VertexPath _roadPath;
        private RoadItem[] _roadItems;

        [Header("Settings")]
        public GameObject[] prefabs;
        public Material[] materials;

        public void BeforeStart(VertexPath roadPath, RoadItem[] roadItems)
        {
            _roadPath = roadPath;
            _roadItems = roadItems;
            _ready = true;
        }

        private IEnumerator Start()
        {
            Debug.Log($"Waiting for princess {GetType().Name}  to be rescued...");
            yield return new WaitUntil(() => _ready);
            Debug.Log($"Princess {GetType().Name}  was rescued!");

            for (var i = 0; i < _roadItems.Length; i++)
            {
                var roadItem = _roadItems[i];
                var position = _roadPath.GetPoint(roadItem.Position.Value);

                var ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                ball.name = "RoadItem_ " + i;
                ball.transform.position = position;
                ball.transform.Translate(Vector3.up * 0.5f);

                var rigidBody = ball.AddComponent<Rigidbody>();
                rigidBody.isKinematic = true;

                var meshRenderer = ball.AddComponent<MeshRenderer>();
                //meshRenderer.material = GameObject.ma
            }
        }
    }
}