using System;
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
        private float _hPos;
        private float _speed;

        [Header("Ball")]
        public EndOfPathInstruction end = EndOfPathInstruction.Stop;

        [Header("Camera")]
        public GameObject ballCamera;
        public Vector3 ballCameraInitPosition = new Vector3(0, 3, -20);
        public Vector3 ballCameraInitRotation = new Vector3(5, 0, 0);

        public void BeforeStart(VertexPath path, float speed)
        {
            _path = path;
            _hPos = 0;
            _speed = speed;

            var ballAction = GetComponent<BallActions>();
            ballAction.BeforeStart();

            _ready = true;
        }

        public void UpdatePosition(int change)
        {
            _hPos = Math.Max(-1f, Math.Min(1f, _hPos + change * 0.1f));
        }

        private void Update()
        {
            StartCoroutine(UpdateBall());
        }

        private IEnumerator UpdateBall()
        {
            yield return new WaitUntil(() => _ready);

            _distanceTraveled += _speed * Time.deltaTime;
            var pathPointPosition = _path.GetPointAtDistance(_distanceTraveled, end);
            var pathPointRotation = _path.GetRotationAtDistance(_distanceTraveled, end);

            // ball
            var ballPosition = pathPointPosition + Vector3.up * 0.5f + Vector3.right * _hPos * 2f;
            var ballRotationAngles = new Vector3(
                transform.rotation.eulerAngles.x,
                pathPointRotation.eulerAngles.y,
                transform.rotation.eulerAngles.z);
            transform.SetPositionAndRotation(ballPosition, Quaternion.Euler(ballRotationAngles));

            // camera
            var cameraPosition = pathPointPosition + Vector3.right * _hPos * 0f;
            var cameraRotationAngles = pathPointRotation.eulerAngles + Vector3.forward * 90 + ballCameraInitRotation;
            ballCamera.transform.SetPositionAndRotation(cameraPosition, Quaternion.Euler(cameraRotationAngles));
            ballCamera.transform.Translate(ballCameraInitPosition);
        }
    }
}