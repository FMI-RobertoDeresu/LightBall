using Assets.Scripts.ServiceModels.ConfigServiceModels.Stages;
using PathCreation;
using UnityEngine;

namespace Assets.Scripts.Modules.Stage
{
    public class RoadManager : MonoBehaviour
    {
        public void RenderRoad(VertexPath roadPath, RoadItem[] roadItems, float startOffset)
        {
            var roadMeshCreator = GetComponent<RoadMeshCreator>();
            roadMeshCreator.CreateMesh(roadPath);

            var roadItemsManager = GetComponent<RoadItemsManager>();
            roadItemsManager.RenderItems(roadPath, roadItems, startOffset);
        }
    }
}