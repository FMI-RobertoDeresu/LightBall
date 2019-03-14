using PathCreation;
using UnityEngine;

namespace Assets.Scripts
{
    public class BallManager : MonoBehaviour
    {
        private float _distanceTraveled;
        private VertexPath _path;

        public float speed = 80;
        public EndOfPathInstruction end = EndOfPathInstruction.Stop;

        public GameObject road;

        public GameObject ballCamera;
        public Vector3 ballCameraInitPosition = new Vector3(0, 3, -20);
        public Vector3 ballCameraInitRotation = new Vector3(5, 0, 0);

        private void Start()
        {
        }

        private void Update()
        {
            _path = _path ?? road.GetComponent<RoadManager>().Path;
            if (_path == null) return;

            _distanceTraveled += speed * Time.deltaTime;
            var pathPointPosition = _path.GetPointAtDistance(_distanceTraveled, end);
            var pathPointRotation = _path.GetRotationAtDistance(_distanceTraveled, end);

            // ball
            transform.position = pathPointPosition;
            transform.Translate(Vector3.up * 0.5f);

            // camera
            var rotationAngles = pathPointRotation.eulerAngles + Vector3.forward * 90 + ballCameraInitRotation;
            ballCamera.transform.SetPositionAndRotation(pathPointPosition, Quaternion.Euler(rotationAngles));
            ballCamera.transform.Translate(ballCameraInitPosition);


            Debug.Log("Euler: " + pathPointRotation.eulerAngles);
        }
    }
}