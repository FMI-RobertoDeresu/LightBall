using System;
using Assets.Scripts.Models.Stages.Enums;
using Assets.Scripts.Utils;
using PathCreation;
using UnityEngine;

namespace Assets.Scripts
{
    public class BallManager : MonoBehaviour
    {
        private bool _ready;
        private bool _canMove;
        private bool _finished;

        private VertexPath _path;
        private float _hPos;
        private float _speed;

        private Action<GameObject> _onBallCollision;
        private Action _onEndPortalReached;

        [Header("Camera")]
        public Camera ballCamera;
        public Vector3 ballCameraInitPosition = new Vector3(0, 3, -20);
        public Vector3 ballCameraInitRotation = new Vector3(5, 0, 0);

        public float DistanceTraveled { get; private set; }

        public RoadItemType CurrentType { get; private set; }

        public void BeforeStart(
            VertexPath path,
            float speed,
            RoadItemType ballType,
            Action<GameObject> onBallCollision,
            Action onEndPortalReached)
        {
            _path = path;
            _hPos = 0;
            _speed = speed;
            CurrentType = ballType;

            _onBallCollision = onBallCollision;
            _onEndPortalReached = onEndPortalReached;

            DistanceTraveled = 15;
            UpdateBallPosition(DistanceTraveled);
            UpdateCameraPosition(DistanceTraveled);

            var ballAction = GetComponent<BallPlayerActions>();
            ballAction.BeforeStart();

            _ready = true;
        }

        public void UpdatePosition(float change)
        {
            if (_canMove == false)
            {
                _canMove = true;
                return;
            }

            _hPos = Math.Max(-1f, Math.Min(1f, _hPos + change));
        }

        private void Update()
        {
            if (!_ready || !_canMove || _finished)
                return;

            DistanceTraveled += _speed * Time.deltaTime;
            UpdateBallPosition(DistanceTraveled);
            UpdateCameraPosition(DistanceTraveled);
        }

        private Vector3 UpdateBallPosition(float distanceTraveled)
        {
            var pathPointPosition = _path.GetPointAtDistance(distanceTraveled);
            var pathPointRotation = _path.GetRotationAtDistance(distanceTraveled);

            var ballPosition = pathPointPosition + Vector3.up * 0.5f + Vector3.right * _hPos * 2f;
            var ballRotationAngles = new Vector3(
                transform.rotation.eulerAngles.x,
                pathPointRotation.eulerAngles.y,
                transform.rotation.eulerAngles.z);
            transform.SetPositionAndRotation(ballPosition, Quaternion.Euler(ballRotationAngles));
            return pathPointPosition;
        }

        private void UpdateCameraPosition(float distanceTraveled)
        {
            var pathPointPosition = _path.GetPointAtDistance(distanceTraveled);
            var pathPointRotation = _path.GetRotationAtDistance(distanceTraveled);

            var cameraPosition = pathPointPosition + Vector3.right * _hPos * 0f;
            var cameraRotationAngles = pathPointRotation.eulerAngles + Vector3.forward * 90 + ballCameraInitRotation;
            ballCamera.transform.SetPositionAndRotation(cameraPosition, Quaternion.Euler(cameraRotationAngles));
            ballCamera.transform.Translate(ballCameraInitPosition);
        }

        public void OnRoadItemBallCollision(GameObject roadItemBall)
        {
            var roadBallType = EnumUtils.Parse<RoadItemType>(roadItemBall.tag);

            _onBallCollision(roadItemBall);

            var meshRenderer = GetComponent<MeshRenderer>();
            var roadItemBallMaterial = roadItemBall.GetComponent<MeshRenderer>().material;
            meshRenderer.material = roadItemBallMaterial;
            CurrentType = roadBallType;

            Destroy(roadItemBall);
        }

        public void OnPortalCollision(GameObject colGameObject)
        {
            _canMove = false;
            _finished = true;
            _onEndPortalReached();
        }
    }
}