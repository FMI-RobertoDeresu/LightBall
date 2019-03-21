﻿using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class BallPlayerActions : MonoBehaviour
    {
        private bool _ready;
        private BallManager _ballManager;

        private float _moveTolerance;
        private float _mouseMoveFactor;
        private float _touchMoveFactor;

        private Vector3? _lastMousePosition;
        private Vector3? _lastTouchPosition;

        public void BeforeStart()
        {
            _ballManager = GetComponent<BallManager>();
            _moveTolerance = 0.1f;
            _mouseMoveFactor = 0.03f;
            _touchMoveFactor = 0.03f;
            _ready = true;
        }

        private void Update()
        {
            if (!_ready)
                return;

            if (Input.mousePresent)
                CheckForMouseActions();

            if (Input.touchSupported)
                CheckForTouchActions();
        }

        private void CheckForMouseActions()
        {
            if (!Input.GetMouseButton(0))
            {
                _lastMousePosition = null;
                return;
            }

            var mousePosition = Input.mousePosition;
            if (_lastMousePosition != null)
            {
                var direction = (mousePosition - _lastMousePosition.Value);
                if (Math.Abs(direction.x) > _moveTolerance)
                {
                    var change = direction.x * _mouseMoveFactor;
                    _ballManager.UpdatePosition(change);
                }
            }

            _lastMousePosition = mousePosition;
        }

        private void CheckForTouchActions()
        {
            if (Input.touchCount <= 0)
            {
                _lastTouchPosition = null;
                return;
            }

            var touchPosition = Input.mousePosition;
            if (_lastMousePosition != null)
            {
                var direction = (touchPosition - _lastTouchPosition.Value);
                if (Math.Abs(direction.x) > _moveTolerance)
                {
                    var change = direction.x * _touchMoveFactor;
                    _ballManager.UpdatePosition(change);
                }
            }

            _lastTouchPosition = touchPosition;
        }
    }
}