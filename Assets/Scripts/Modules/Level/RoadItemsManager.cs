using System.Linq;
using Assets.Scripts.Extensions;
using Assets.Scripts.ServiceModels.ConfigServiceModels.Levels;
using Assets.Scripts.ServiceModels.ConfigServiceModels.Levels.Enums;
using PathCreation;
using UnityEngine;

namespace Assets.Scripts.Modules.Level
{
    public class RoadItemsManager : MonoBehaviour
    {
        [Header("Settings")]
        public GameObject[] prefabs;
        public Material[] materials;

        public void RenderItems(VertexPath roadPath, RoadConfig roadConfig)
        {
            var startOffsetPosition = Mathf.Clamp01(roadConfig.ItemsStartOffset.Value / roadPath.length);
            var endOffsetPosition = Mathf.Clamp01(roadConfig.ItemsEndOffset.Value / roadPath.length);
            var innerPathPercent = Mathf.Clamp01(1 - endOffsetPosition - startOffsetPosition);

            for (var i = 0; i < roadConfig.Items.Length; i++)
            {
                var roadItem = roadConfig.Items[i];
                var roadItemPosition = roadItem.Position.Value * innerPathPercent + startOffsetPosition;
                var pointPosition = roadPath.GetPoint(roadItemPosition);
                var pointRotationAngles = roadPath.GetRotation(roadItemPosition).eulerAngles;

                // instantiate
                var prefabName = roadItem.Type.Value.GetPrefabName();
                var prefab = prefabs.First(x => x.name == prefabName);

                var roadItemGo = Instantiate(prefab);
                roadItemGo.name = "RoadItem_ " + i;
                roadItemGo.tag = roadItem.Type.ToString();
                roadItemGo.layer = roadItem.Type.Value.GetLayerValue();

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