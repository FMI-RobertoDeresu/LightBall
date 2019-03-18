using System.Collections;
using Assets.Scripts.Extensions;
using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class BallActions : MonoBehaviour
    {
        private bool _ready;
        private BallManager _ballManager;

        public void BeforeStart()
        {
            _ballManager = GetComponent<BallManager>();
            _ready = true;
        }

        private void Update()
        {
            StartCoroutine(ListenForActions());
        }

        private IEnumerator ListenForActions()
        {
            yield return new WaitUntil(() => _ready);

            if (Input.GetKeyDown(KeyCode.LeftArrow))
                _ballManager.ChangeSide(-1);

            if (Input.GetKeyDown(KeyCode.RightArrow))
                _ballManager.ChangeSide(1);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_ready)
                return;

            Debug.Log("Press position + " + eventData.pressPosition);
            Debug.Log("End position + " + eventData.position);
            Vector3 dragVectorDirection = (eventData.position - eventData.pressPosition).normalized;
            Debug.Log("norm + " + dragVectorDirection);
            var direction = GetSwipeDirection(dragVectorDirection);

            if (direction.IsIn(SwipeDirection.Left, SwipeDirection.Right))
            {
                var intDirection = direction == SwipeDirection.Left ? -1 : 1;
                _ballManager.ChangeSide(intDirection);
            }
        }

        private SwipeDirection GetSwipeDirection(Vector3 dragVector)
        {
            var positiveX = Mathf.Abs(dragVector.x);
            var positiveY = Mathf.Abs(dragVector.y);
            SwipeDirection swipeDir;
            if (positiveX > positiveY)
                swipeDir = dragVector.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
            else
                swipeDir = dragVector.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;
            Debug.Log(swipeDir);
            return swipeDir;
        }
    }
}