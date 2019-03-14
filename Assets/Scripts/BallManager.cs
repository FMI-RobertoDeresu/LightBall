using System.Collections;
using PathCreation;
using UnityEngine;

namespace Assets.Scripts
{
    public class BallManager : MonoBehaviour
    {
        private bool _ready;
        private float _distanceTraveled;
        private VertexPath _path;

        [Header("Ball")]
        public float speed = 80;
        public EndOfPathInstruction end = EndOfPathInstruction.Stop;

        [Header("Camera")]
        public GameObject ballCamera;
        public Vector3 ballCameraInitPosition = new Vector3(0, 3, -20);
        public Vector3 ballCameraInitRotation = new Vector3(5, 0, 0);

        private IEnumerator Start()
        {
            Debug.Log($"Waiting for princess {GetType().Name}  to be rescued...");
            yield return new WaitUntil(() => _ready);
            Debug.Log($"Princess {GetType().Name}  was rescued!");
        }

        private void Update()
        {
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
        }

        public void BeforeStart(VertexPath path)
        {
            _path = path;
            _ready = true;
        }
    }
}