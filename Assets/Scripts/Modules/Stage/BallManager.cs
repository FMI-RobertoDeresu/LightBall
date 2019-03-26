using System;
using System.Linq;
using Assets.Scripts.Extensions;
using Assets.Scripts.Models.Config.Stages.Enums;
using Assets.Scripts.Utils;
using PathCreation;
using UnityEngine;

namespace Assets.Scripts.Modules.Stage
{
    public class BallManager : MonoBehaviour
    {
        private bool _ready;
        private bool _canMove;
        private bool _finished;

        private VertexPath _path;
        private float _onRoadPos;
        private float _speed;
        private RoadItemType _ballType;
        private float _startOffset;

        private Action<GameObject> _onBallCollision;
        private Action _onEndPortalReached;

        [Header("Ball")]
        public bool waitForTouch = true;
        public Material[] materials;

        [Header("Camera")]
        public Camera ballCamera;
        public Vector3 ballCameraInitPosition = new Vector3(0, 3, -20);
        public Vector3 ballCameraInitRotation = new Vector3(5, 0, 0);

        public float DistanceTraveled { get; private set; }
        public RoadItemType CurrentType { get; private set; }
        public Vector3 Position { get; private set; }

        public void BeforeStart(
            VertexPath path,
            float speed,
            RoadItemType ballType,
            float startOffset,
            Action<GameObject> onBallCollision,
            Action onEndPortalReached)
        {
            _onRoadPos = 0;
            DistanceTraveled = 0;

            _path = path;
            _speed = speed;
            _ballType = ballType;
            _startOffset = startOffset;
            _onBallCollision = onBallCollision;
            _onEndPortalReached = onEndPortalReached;

            CurrentType = _ballType;
            UpdateBallPosition(DistanceTraveled);
            UpdateCameraPosition(DistanceTraveled);

            var ballAction = GetComponent<BallPlayerActions>();
            ballAction.BeforeStart();

            _canMove = !waitForTouch;
            _ready = true;
        }

        public void UpdatePosition(float change)
        {
            if (_canMove == false)
            {
                _canMove = true;
                return;
            }

            _onRoadPos = Math.Max(-1f, Math.Min(1f, _onRoadPos + change));
        }

        private void Update()
        {
            if (!_ready || !_canMove || _finished)
                return;

            DistanceTraveled += _speed * Time.deltaTime;
            UpdateBallPosition(DistanceTraveled);
            UpdateCameraPosition(DistanceTraveled);
        }

        private void UpdateBallPosition(float distanceTraveled)
        {
            var pathPointPosition = _path.GetPointAtDistance(distanceTraveled + _startOffset);
            var pathPointRotation = _path.GetRotationAtDistance(distanceTraveled + _startOffset);

            var ballPosition = pathPointPosition + Vector3.up * 0.5f + Vector3.right * _onRoadPos * 2f;
            var ballRotationAngles = new Vector3(
                transform.rotation.eulerAngles.x,
                pathPointRotation.eulerAngles.y,
                transform.rotation.eulerAngles.z);
            transform.SetPositionAndRotation(ballPosition, Quaternion.Euler(ballRotationAngles));
            Position = ballPosition;
        }

        private void UpdateCameraPosition(float distanceTraveled)
        {
            var pathPointPosition = _path.GetPointAtDistance(distanceTraveled + _startOffset);
            var pathPointRotation = _path.GetRotationAtDistance(distanceTraveled + _startOffset);

            var cameraPosition = pathPointPosition + Vector3.right * _onRoadPos * 0f;
            var cameraRotationAngles = pathPointRotation.eulerAngles + Vector3.forward * 90 + ballCameraInitRotation;
            ballCamera.transform.SetPositionAndRotation(cameraPosition, Quaternion.Euler(cameraRotationAngles));
            ballCamera.transform.Translate(ballCameraInitPosition);
        }

        public void OnRoadItemBallCollision(GameObject roadItemBall)
        {
            _onBallCollision(roadItemBall);
        }

        public void OnRoadItemSwitchCollision(GameObject roadItemSwitch)
        {
            var roadSwitchType = EnumUtils.Parse<RoadItemType>(roadItemSwitch.tag);
            var meshRenderer = GetComponent<MeshRenderer>();

            var ballTypeAfterSwitch = roadSwitchType.GetSwitchBallType();
            var materialName = ballTypeAfterSwitch.GetMaterialName();
            var roadItemBallMaterial = materials.First(x => x.name == materialName);

            meshRenderer.material = roadItemBallMaterial;
            CurrentType = ballTypeAfterSwitch;
        }

        public void OnPortalCollision(GameObject colGameObject)
        {
            _canMove = false;
            _finished = true;
            _onEndPortalReached();
        }
    }
}