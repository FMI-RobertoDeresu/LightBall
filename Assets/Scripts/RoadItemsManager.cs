using System.Linq;
using Assets.Scripts.Extensions;
using Assets.Scripts.Models.Stages;
using Assets.Scripts.Models.Stages.Enums;
using PathCreation;
using UnityEngine;

namespace Assets.Scripts
{
    public class RoadItemsManager : MonoBehaviour
    {
        [Header("Settings")]
        public GameObject[] prefabs;
        public Material[] materials;

        public void RenderItems(VertexPath roadPath, RoadItem[] roadItems)
        {
            for (var i = 0; i < roadItems.Length; i++)
            {
                var roadItem = roadItems[i];
                var roadItemPosition = roadItem.Position.Value;
                var pointPosition = roadPath.GetPoint(roadItemPosition);
                var pointRotationAngles = roadPath.GetRotation(roadItemPosition).eulerAngles;

                // instantiate
                var prefabName = roadItem.Type.Value.GetPrefabName();
                var prefab = prefabs.First(x => x.name == prefabName);

                var roadItemGo = Instantiate(prefab);
                roadItemGo.name = "RoadItem_ " + i;
                roadItemGo.tag = roadItem.Type.ToString();

                // position and rotation
                var side = (roadItem.Side == RoadItemSide.Left ? -1f : 0f) * 2 +
                           (roadItem.Side == RoadItemSide.Right ? 1f : 0f) * 2;
                var position = pointPosition + Vector3.right * side + Vector3.up * 0.5f;
                var rotation = Quaternion.Euler(new Vector3(pointRotationAngles.x, pointRotationAngles.y, 0));
                roadItemGo.transform.SetPositionAndRotation(position, rotation);

                // mesh
                var materialName = roadItem.Type.Value.GetMaterialName();
                if (!string.IsNullOrEmpty(materialName))
                {
                    var meshRenderer = roadItemGo.GetComponent<MeshRenderer>();
                    var material = materials.First(x => x.name == materialName);
                    meshRenderer.material = material;
                }
            }
        }
    }
}