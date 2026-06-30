using System.Collections.Generic;
using UnityEngine;

namespace MapModule
{
    /// <summary>
    /// Class that generates a plane with the given parameters
    /// </summary>
    public class PlaneGenerator : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Determines how big the plane is")]
        private Vector2 planeSize = Vector2.one;

        [SerializeField]
        [Min(0.1f)]
        [Tooltip("Determines how precised the mesh is")]
        private float resolution = 1;

        /// <inheritdoc cref="Start" />
        private void Start()
        {
            if (!TryGetComponent(out MeshFilter meshFilter))
            {
                Debug.LogWarning($"No MeshFilter found in '{gameObject.name}'.");
                return;
            }

            meshFilter.mesh = GenerateMesh(planeSize, resolution);
        }

        /// <summary>
        /// Generates a mesh for a plane with the given size and the given resolution
        /// </summary>
        /// <param name="size">Size of the plane</param>
        /// <param name="resolution">Resolution of the plane</param>
        /// <param name="name">Name of the mesh</param>
        /// <returns>Generated mesh</returns>
        private static Mesh GenerateMesh(Vector2 size, float resolution = 1, string name = null)
        {
            // Create mesh
            var scaledSize = new Vector2Int(
                Mathf.FloorToInt(size.x / resolution),
                Mathf.FloorToInt(size.y / resolution)
            );

            Vector3[] vertices = GenerateVertices(scaledSize,
                resolution,
                new Vector3(
                    Mathf.FloorToInt(-scaledSize.x / 2f),
                    0,
                    Mathf.FloorToInt(-scaledSize.y / 2f)
                ));
            int[] triangles = GenerateTriangles(scaledSize);
            var uvs = new Vector2[vertices.Length];

            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    uvs[(int)(x + y * size.x)] = new Vector2(
                        x / size.x,
                        y / size.y
                    );
                }
            }

            var mesh = new Mesh
            {
                name = name ?? $"Generated Plane ({scaledSize.x}x{scaledSize.y})",
            };

            // Assign mesh
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            mesh.RecalculateNormals();

            return mesh;
        }

        /// <summary>
        /// Generates the vertices for a plane of the given size and the given resolution
        /// </summary>
        /// <param name="size">Size of the plane</param>
        /// <param name="resolution">How detailed is the plane</param>
        /// <param name="offset">Offset of each vertice</param>
        /// <returns>Generated vertices</returns>
        private static Vector3[] GenerateVertices(Vector2Int size, float resolution, Vector3? offset = null)
        {
            int totalAmount = (size.x + 1) * (size.y + 1);
            var vertices = new Vector3[totalAmount];

            offset ??= Vector3.zero;

            for (int i = 0; i < vertices.Length; i++)
            {
                int y = i / (size.y + 1);
                int x = i % (size.x + 1);

                vertices[i] = (new Vector3(x, 0, y) + offset.Value) * resolution;
            }

            return vertices;
        }

        /// <summary>
        /// Generates the triangles for a plane of the given size
        /// </summary>
        /// <param name="size">Size of the plane</param>
        /// <returns>Generated triangles</returns>
        private static int[] GenerateTriangles(Vector2Int size)
        {
            var triangles = new List<int>();

            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    // Create triangles
                    int i = x + (size.x + 1) * y;
                    int sharedPoint = i + size.x + 1;

                    // First
                    triangles.Add(sharedPoint);
                    triangles.Add(i + 1);
                    triangles.Add(i);

                    // Second
                    triangles.Add(i + 1);
                    triangles.Add(sharedPoint);
                    triangles.Add(sharedPoint + 1);
                }
            }

            return triangles.ToArray();
        }
    }
}