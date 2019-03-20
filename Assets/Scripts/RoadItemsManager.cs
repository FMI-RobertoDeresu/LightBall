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
                var pointRotationAngles = roadPath.GetRotation(roadItem.Position.Value).eulerAngles;

                // ball
                if (roadItem.Type.IsIn(RoadItemType.RedBall, RoadItemType.BlueBall, RoadItemType.YellowBall,
                    RoadItemType.PurpleBall))
                {
                    var ballPrefab = prefabs.First(x => x.name == "Sphere");
                    var ballColor = roadItem.Type.ToString().Replace("Ball", "");

                    var ball = Instantiate(ballPrefab);
                    roadItemGo = ball;

                    ball.name = "RoadItem_Ball_ " + i;
                    ball.tag = $"{ballColor}Ball";
                    ball.transform.position = pointPosition;
                    ball.transform.Translate(Vector3.up * 0.5f);

                    var meshRenderer = ball.GetComponent<MeshRenderer>();
                    var materialName = $"Ball {ballColor}";
                    var material = materials.First(x => x.name == materialName);
                    meshRenderer.material = material;
                }

                // common
                if (roadItemGo != null)
                {
                    var side = (roadItem.Side == RoadItemSide.Left ? -1f : 0f) * 2 +
                               (roadItem.Side == RoadItemSide.Right ? 1f : 0f) * 2;

                    var position = pointPosition + Vector3.right * side + Vector3.up * 0.5f;
                    var rotation = Quaternion.Euler(new Vector3(pointRotationAngles.x, pointRotationAngles.y, 0));
                    roadItemGo.transform.SetPositionAndRotation(position, rotation);
                }
            }
        }

        public void RenderPortal(VertexPath roadPath)
        {
            var portalPrefab = prefabs.First(x => x.name == "Portal");
            var portal = Instantiate(portalPrefab);

            portal.name = "RoadItem_Portal";

            var pointPosition = roadPath.GetPoint(0.999f);
            var pointRotationAngles = roadPath.GetRotation(0.999f).eulerAngles;
            portal.transform.SetPositionAndRotation(pointPosition, Quaternion.Euler(pointRotationAngles));
        }
    }
}