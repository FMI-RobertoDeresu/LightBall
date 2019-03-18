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
                var roadItemGo = (GameObject) null;
                var pointPosition = roadPath.GetPoint(roadItem.Position.Value);
                var pointRotation = roadPath.GetRotation(roadItem.Position.Value);

                // ball
                if (roadItem.Type.IsIn(RoadItemType.RedBall, RoadItemType.BlueBall, RoadItemType.YellowBall,
                    RoadItemType.PurpleBall))
                {
                    var sphere = prefabs.First(x => x.name == "Sphere");
                    var ball = Instantiate(sphere);
                    roadItemGo = ball;

                    ball.name = "RoadItem_ " + i;
                    ball.transform.position = pointPosition;
                    ball.transform.Translate(Vector3.up * 0.5f);

                    var meshRenderer = ball.GetComponent<MeshRenderer>();
                    var materialName = roadItem.Type.ToString().Replace("Ball", "");
                    var material = materials.First(x => x.name == materialName);
                    meshRenderer.material = material;
                }

                // common
                if (roadItemGo != null)
                {
                    var side = (roadItem.Side == RoadItemSide.Left ? -1f : 0f) * 2 +
                                         (roadItem.Side == RoadItemSide.Right ? 1f : 0f) * 2;
                    roadItemGo.transform.position = pointPosition + Vector3.right * side + Vector3.up * 0.5f;
                    roadItemGo.transform.rotation = pointRotation;
                }
            }
        }
    }
}