using Assets.Scripts.ServiceModels.ConfigServiceModels.Levels;
using PathCreation;
using UnityEngine;

namespace Assets.Scripts.Modules.Level
{
    public class RoadManager : MonoBehaviour
    {
        public void RenderRoad(VertexPath roadPath, RoadConfig roadConfig)
        {
            var roadMeshCreator = GetComponent<RoadMeshCreator>();
            roadMeshCreator.CreateMesh(roadPath);

            var roadItemsManager = GetComponent<RoadItemsManager>();
            roadItemsManager.RenderItems(roadPath, roadConfig);
        }
    }
}