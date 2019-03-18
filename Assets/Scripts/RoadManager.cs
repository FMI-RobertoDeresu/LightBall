using Assets.Scripts.Models.Stages;
using PathCreation;
using UnityEngine;

namespace Assets.Scripts
{
    public class RoadManager : MonoBehaviour
    {
        public void RenderRoad(VertexPath roadPath, RoadItem[] roadItems)
        {
            var roadMeshCreator = GetComponent<RoadMeshCreator>();
            roadMeshCreator.CreateMesh(roadPath);

            var roadItemsManager = GetComponent<RoadItemsManager>();
            roadItemsManager.RenderItems(roadPath, roadItems);
        }
    }
}