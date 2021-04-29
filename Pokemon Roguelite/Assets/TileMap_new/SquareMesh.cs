using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SquareMesh : MonoBehaviour
{
    Mesh squareMesh;
    MeshCollider meshCollider;

    static List<Vector3> verticies = new List<Vector3>();
    static List<int> triangles = new List<int>();
    static List<Color> colors = new List<Color>();

    public bool useTerrainTypes;
    [NonSerialized] List<Vector3> verticiesTerrain, terrainTypes;


    private void Awake()
    {
        GetComponent<MeshFilter>().mesh = squareMesh = new Mesh();
        meshCollider = gameObject.AddComponent<MeshCollider>();
        squareMesh.name = "Square Mesh";
    }

    public void Clear() 
    {
        squareMesh.Clear();
        verticies.Clear();
        colors.Clear();
        triangles.Clear();
    }

    public void Apply()
    {
        squareMesh.SetVertices(verticies);
        squareMesh.SetColors(colors);
        squareMesh.SetTriangles(triangles, 0);
        squareMesh.RecalculateNormals();
        meshCollider.sharedMesh = squareMesh;
    }

    public void AddTriangleColor(Color color) 
    {
        colors.Add(color);
        colors.Add(color);
        colors.Add(color);
    }

    public void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3) 
    {
        int vertexIndex = verticies.Count;
        verticies.Add(v1);
        verticies.Add(v2);
        verticies.Add(v3);

        triangles.Add(vertexIndex + 0);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
    }

    public void AddQuadColor(Color c)
    {
        colors.Add(c);
        colors.Add(c);
        colors.Add(c);
        colors.Add(c);
    }

    public void AddQuad(Vector3 topLeft, Vector3 topRight, Vector3 bottomRight, Vector3 bottomLeft) 
    {
        int vertexIndex = verticies.Count;
        verticies.Add(topLeft);
        verticies.Add(topRight);
        verticies.Add(bottomRight);
        verticies.Add(bottomLeft);

        // Tri #1
        triangles.Add(vertexIndex + 0);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);

        // #2
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 3);
        triangles.Add(vertexIndex + 0);
    }

}
