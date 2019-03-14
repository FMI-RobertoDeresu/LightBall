using PathCreation;
using UnityEngine;

namespace Assets.Scripts
{
    public class RoadManager : MonoBehaviour
    {
        private bool _generated;

        public Vector3[] points;
        public VertexPath Path { get; private set; }

        private void Start()
        {
            var firstFactor = 100;
            var secondFactor = 100;
            points = new[]
            {
                Vector3.zero,
                //Vector3.forward * 10 * 1,
                Vector3.forward * firstFactor * 2 + Vector3.left * secondFactor * 1,
                Vector3.forward * firstFactor * 3,
                Vector3.forward * firstFactor * 4 + Vector3.right * secondFactor * 1,
                Vector3.forward * firstFactor * 5 + Vector3.up * secondFactor * 1,
                Vector3.forward * firstFactor * 6,
                Vector3.forward * firstFactor * 7 + Vector3.right * secondFactor * 1
                //Vector3.forward * 10 * 7,
                //Vector3.forward * 10 * 8,
                //Vector3.forward * 10 * 9,
                //Vector3.forward * 10 * 10
            };

            var bezierPath = new BezierPath(points);
            Path = new VertexPath(bezierPath);

            var roadMeshCreator = GetComponent<RoadMeshCreator>();
            roadMeshCreator.Create(Path);

            for (var i = 0; i < Path.NumVertices; i++)
            {
                var distance = Path.cumulativeLengthAtEachVertex[i];
                var position = Path.GetPointAtDistance(distance);
                var rotation = Path.GetRotationAtDistance(distance);

                var sphere2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere2.name = "Rotation " + i;
                sphere2.transform.position = position;
                sphere2.transform.rotation = rotation;
                sphere2.transform.Translate(Vector3.up * 2);
            }

            Debug.Log(Path.normals);
            Debug.Log(Path.tangents);
        }

        private void Update()
        {
        }
    }
}