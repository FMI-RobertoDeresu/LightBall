using System.Collections;
using PathCreation;
using UnityEngine;

namespace Assets.Scripts
{
    public class RoadMeshCreator : MonoBehaviour
    {
        private bool _ready;
        private VertexPath _roadPath;

        [Header("Road settings")]
        public float roadWidth = .4f;
        [Range(0, .5f)]
        public float thickness = .15f;
        public bool flattenSurface;

        [Header("Material settings")]
        public Material roadMaterial;
        public Material undersideMaterial;
        public float textureTiling = 1;

        public void BeforeStart(VertexPath roadPath)
        {
            _roadPath = roadPath;
            _ready = true;
        }

        private IEnumerator Start()
        {
            Debug.Log($"Waiting for princess {GetType().Name}  to be rescued...");
            yield return new WaitUntil(() => _ready);
            Debug.Log($"Princess {GetType().Name}  was rescued!");

            var (meshFilter, meshRenderer) = GetMeshComponents();
            AssignMaterials(meshRenderer);
            meshFilter.mesh = CreateRoadMesh(_roadPath);
        }

        private (MeshFilter, MeshRenderer) GetMeshComponents()
        {
            // Find/creator mesh holder object in children
            var meshHolderName = "MeshHolder";
            var meshHolder = transform.Find(meshHolderName);

            //meshHolder.transform.position = Vector3.zero;
            meshHolder.transform.rotation = Quaternion.identity;

            // Ensure mesh renderer and filter components are assigned
            if (!meshHolder.gameObject.GetComponent<MeshFilter>())
                meshHolder.gameObject.AddComponent<MeshFilter>();

            if (!meshHolder.GetComponent<MeshRenderer>())
                meshHolder.gameObject.AddComponent<MeshRenderer>();

            var meshFilter = meshHolder.GetComponent<MeshFilter>();
            var meshRenderer = meshHolder.GetComponent<MeshRenderer>();

            return (meshFilter, meshRenderer);
        }

        private void AssignMaterials(MeshRenderer meshRenderer)
        {
            if (roadMaterial != null && undersideMaterial != null)
            {
                meshRenderer.sharedMaterials = new[] {roadMaterial, undersideMaterial, undersideMaterial};
                meshRenderer.sharedMaterials[0].mainTextureScale = new Vector3(1, textureTiling);
            }
        }

        private Mesh CreateRoadMesh(VertexPath path)
        {
            var vertices = new Vector3[path.NumVertices * 8];
            var uvs = new Vector2[vertices.Length];
            var normals = new Vector3[vertices.Length];

            var numTriangles = 2 * (path.NumVertices - 1) + (path.isClosedLoop ? 2 : 0);
            var roadTriangles = new int[numTriangles * 3];
            var underRoadTriangles = new int[numTriangles * 3];
            var sideOfRoadTriangles = new int[numTriangles * 2 * 3];

            var vertexIndex = 0;
            var triIndex = 0;

            // Vertices for the top of the road are layed out:
            // 0  1
            // 8  9
            // and so on... So the triangle map 0,8,1 for example, defines a triangle from top left to bottom left to bottom right.
            int[] triangleMap = {0, 8, 1, 1, 8, 9};
            int[] sidesTriangleMap = {4, 6, 14, 12, 4, 14, 5, 15, 7, 13, 15, 5};

            var usePathNormals = !(path.space == PathSpace.xyz && flattenSurface);

            for (var i = 0; i < path.NumVertices; i++)
            {
                var localUp = usePathNormals ? Vector3.Cross(path.tangents[i], path.normals[i]) : path.up;
                var localRight = usePathNormals ? path.normals[i] : Vector3.Cross(localUp, path.tangents[i]);

                // Find position to left and right of current path vertex
                var vertexSideA = path.vertices[i] - localRight * Mathf.Abs(roadWidth) - transform.position;
                var vertexSideB = path.vertices[i] + localRight * Mathf.Abs(roadWidth) - transform.position;

                // Add top of road vertices
                vertices[vertexIndex + 0] = vertexSideA;
                vertices[vertexIndex + 1] = vertexSideB;
                // Add bottom of road vertices
                vertices[vertexIndex + 2] = vertexSideA - localUp * thickness;
                vertices[vertexIndex + 3] = vertexSideB - localUp * thickness;

                // Duplicate vertices to get flat shading for sides of road
                vertices[vertexIndex + 4] = vertices[vertexIndex + 0];
                vertices[vertexIndex + 5] = vertices[vertexIndex + 1];
                vertices[vertexIndex + 6] = vertices[vertexIndex + 2];
                vertices[vertexIndex + 7] = vertices[vertexIndex + 3];

                // Set uv on y axis to path time (0 at start of path, up to 1 at end of path)
                uvs[vertexIndex + 0] = new Vector2(0, path.times[i]);
                uvs[vertexIndex + 1] = new Vector2(1, path.times[i]);

                // Top of road normals
                normals[vertexIndex + 0] = localUp;
                normals[vertexIndex + 1] = localUp;
                // Bottom of road normals
                normals[vertexIndex + 2] = -localUp;
                normals[vertexIndex + 3] = -localUp;
                // Sides of road normals
                normals[vertexIndex + 4] = -localRight;
                normals[vertexIndex + 5] = localRight;
                normals[vertexIndex + 6] = -localRight;
                normals[vertexIndex + 7] = localRight;

                // Set triangle indices
                if (i < path.NumVertices - 1 || path.isClosedLoop)
                {
                    for (var j = 0; j < triangleMap.Length; j++)
                    {
                        roadTriangles[triIndex + j] = (vertexIndex + triangleMap[j]) % vertices.Length;
                        // reverse triangle map for under road so that triangles wind the other way and are visible from underneath
                        underRoadTriangles[triIndex + j] =
                            (vertexIndex + triangleMap[triangleMap.Length - 1 - j] + 2) % vertices.Length;
                    }

                    for (var j = 0; j < sidesTriangleMap.Length; j++)
                        sideOfRoadTriangles[triIndex * 2 + j] = (vertexIndex + sidesTriangleMap[j]) % vertices.Length;
                }

                vertexIndex += 8;
                triIndex += 6;
            }

            var mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.uv = uvs;
            mesh.normals = normals;
            mesh.subMeshCount = 3;
            mesh.SetTriangles(roadTriangles, 0);
            mesh.SetTriangles(underRoadTriangles, 1);
            mesh.SetTriangles(sideOfRoadTriangles, 2);
            mesh.RecalculateBounds();

            return mesh;
        }
    }
}