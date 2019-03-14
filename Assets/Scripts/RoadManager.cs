using System.Collections;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Stages;
using PathCreation;
using UnityEngine;

namespace Assets.Scripts
{
    public class RoadManager : MonoBehaviour
    {
        private bool _ready;
        private VertexPath _roadPath;
        private RoadItem[] _roadItems;

        public void BeforeStart(VertexPath roadPath, RoadItem[] roadItems)
        {
            _roadPath = roadPath;
            _roadItems = roadItems;
            _ready = true;
        }

        private IEnumerator Start()
        {
            Debug.Log($"Waiting for princess {GetType().Name}  to be rescued...");
            yield return new WaitUntil(() => _ready);
            Debug.Log($"Princess {GetType().Name}  was rescued!");

            var roadMeshCreator = GetComponent<RoadMeshCreator>();
            roadMeshCreator.BeforeStart(_roadPath);

            var roadItemsManager = GetComponent<RoadItemsManager>();
            roadItemsManager.BeforeStart(_roadPath, _roadItems);
        }
    }
}