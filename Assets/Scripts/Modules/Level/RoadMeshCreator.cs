﻿using PathCreation;
using UnityEngine;

namespace Assets.Scripts.Modules.Level
{
    public class RoadMeshCreator : MonoBehaviour
    {
        [Header("Road settings")]
        public float roadWidth = .4f;
        [Range(0, .5f)]
        public float thickness = .15f;
        public bool flattenSurface;

        [Header("Material settings")]
        public Material roadMaterial;
        public Material underMaterial;
        public Material sideMaterial;

        public void CreateMesh(VertexPath roadPath)
        {
            //transform.rotation = Quaternion.identity;
            var meshFilter = GetComponent<MeshFilter>();
            var meshRenderer = GetComponent<MeshRenderer>();
            var meshCollider = GetComponent<MeshCollider>();

            meshRenderer.sharedMaterials = new[] { roadMaterial, underMaterial, sideMaterial };

            var mesh = CreateRoadMesh(roadPath);

            meshFilter.mesh = mesh;
            meshCollider.sharedMesh = mesh;
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

            // Vertices for the top of the roadGo are layed out:
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

                // Add top of roadGo vertices
                vertices[vertexIndex + 0] = vertexSideA;
                vertices[vertexIndex + 1] = vertexSideB;
                // Add bottom of roadGo vertices
                vertices[vertexIndex + 2] = vertexSideA - localUp * thickness;
                vertices[vertexIndex + 3] = vertexSideB - localUp * thickness;

                // Duplicate vertices to get flat shading for sides of roadGo
                vertices[vertexIndex + 4] = vertices[vertexIndex + 0];
                vertices[vertexIndex + 5] = vertices[vertexIndex + 1];
                vertices[vertexIndex + 6] = vertices[vertexIndex + 2];
                vertices[vertexIndex + 7] = vertices[vertexIndex + 3];

                // Set uv on y axis to path time (0 at start of path, up to 1 at end of path)
                uvs[vertexIndex + 0] = new Vector2(0, path.times[i]);
                uvs[vertexIndex + 1] = new Vector2(1, path.times[i]);

                // Top of roadGo normals
                normals[vertexIndex + 0] = localUp;
                normals[vertexIndex + 1] = localUp;
                // Bottom of roadGo normals
                normals[vertexIndex + 2] = -localUp;
                normals[vertexIndex + 3] = -localUp;
                // Sides of roadGo normals
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
                        // reverse triangle map for under roadGo so that triangles wind the other way and are visible from underneath
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