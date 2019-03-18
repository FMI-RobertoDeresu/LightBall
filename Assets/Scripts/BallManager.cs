using System.Collections;
using Assets.Scripts.Models;
using PathCreation;
using UnityEngine;

namespace Assets.Scripts
{
    public class BallManager : MonoBehaviour
    {
        private bool _ready;
        private float _distanceTraveled;
        private VertexPath _path;
        private float _speed;
        private float _rotationSpeed;
        private BallSide _side;

        [Header("Ball")]
        public EndOfPathInstruction end = EndOfPathInstruction.Stop;

        [Header("Camera")]
        public GameObject ballCamera;
        public Vector3 ballCameraInitPosition = new Vector3(0, 3, -20);
        public Vector3 ballCameraInitRotation = new Vector3(5, 0, 0);

        public void BeforeStart(VertexPath path)
        {
            _path = path;
            _speed = 15;
            _rotationSpeed = 15;
            _side = BallSide.Center;

            var ballAction = GetComponent<BallActions>();
            ballAction.BeforeStart();

            _ready = true;
        }

        public void ChangeSide(int change)
        {
            if (_side == BallSide.Left && change == -1 || _side == BallSide.Right && change == 1)
                return;
            _side = _side + change;
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
            var ballPosition = pathPointPosition + Vector3.up * 0.5f + Vector3.right * ((int) _side - 2f) * 2f;
            var ballRotationAngles = new Vector3(
                pathPointRotation.eulerAngles.x,
                90 + pathPointRotation.eulerAngles.y,
                transform.rotation.eulerAngles.z + _rotationSpeed);
            transform.SetPositionAndRotation(ballPosition, Quaternion.Euler(ballRotationAngles));

            // camera
            var cameraPosition = pathPointPosition + Vector3.right * ((int) _side - 2f) * 0f;
            var cameraRotationAngles = pathPointRotation.eulerAngles + Vector3.forward * 90 + ballCameraInitRotation;
            ballCamera.transform.SetPositionAndRotation(cameraPosition, Quaternion.Euler(cameraRotationAngles));
            ballCamera.transform.Translate(ballCameraInitPosition);
        }
    }
}