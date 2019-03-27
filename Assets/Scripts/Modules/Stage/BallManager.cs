using System;
using System.Linq;
using Assets.Scripts.Extensions;
using Assets.Scripts.ServiceModels.ConfigServiceModels.Stages;
using Assets.Scripts.ServiceModels.ConfigServiceModels.Stages.Enums;
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
        private BallConfig _config;
        private Action<GameObject> _onBallCollision;
        private Action<GameObject> _onBallOvercome;
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

        public void Init(VertexPath path, BallConfig config, Action<GameObject> onBallCollision, Action<GameObject> onBallOvercome, Action onEndPortalReached)
        {
            _onRoadPos = 0;
            DistanceTraveled = 0;

            _path = path;
            _config = config;
            _onBallCollision = onBallCollision;
            _onBallOvercome = onBallOvercome;
            _onEndPortalReached = onEndPortalReached;

            CurrentType = config.InitialType.Value;
            UpdateBallPosition(DistanceTraveled);
            UpdateCameraPosition(DistanceTraveled);

            _canMove = !waitForTouch;
            _ready = true;
        }

        private void Update()
        {
            if (!_ready || !_canMove || _finished)
                return;

            DistanceTraveled += _config.Speed.Value * 0.01f;
            UpdateBallPosition(DistanceTraveled);
            UpdateCameraPosition(DistanceTraveled);
        }

        private void UpdateBallPosition(float distanceTraveled)
        {
            var pathPointPosition = _path.GetPointAtDistance(distanceTraveled + _config.StartOffset.Value);
            var pathPointRotation = _path.GetRotationAtDistance(distanceTraveled + _config.StartOffset.Value);

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
            var pathPointPosition = _path.GetPointAtDistance(distanceTraveled + _config.StartOffset.Value);
            var pathPointRotation = _path.GetRotationAtDistance(distanceTraveled + _config.StartOffset.Value);

            var cameraPosition = pathPointPosition + Vector3.right * _onRoadPos * 0f;
            var cameraRotationAngles = pathPointRotation.eulerAngles + Vector3.forward * 90 + ballCameraInitRotation;
            ballCamera.transform.SetPositionAndRotation(cameraPosition, Quaternion.Euler(cameraRotationAngles));
            ballCamera.transform.Translate(ballCameraInitPosition);
        }

        public void OnUpdatePosition(float change)
        {
            if (!_canMove && !_finished)
            {
                _canMove = true;
                return;
            }

            _onRoadPos = Math.Max(-1f, Math.Min(1f, _onRoadPos + change));
        }

        public void OnBallCollision(GameObject roadItemBall)
        {
            _onBallCollision(roadItemBall);
        }

        public void OnBallOvercome(GameObject roadItemBall)
        {
            _onBallOvercome(roadItemBall);
        }

        public void OnSwitchCollision(GameObject roadItemSwitch)
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