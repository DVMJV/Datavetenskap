using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SquareMesh : MonoBehaviour
{
    Mesh squareMesh;
    MeshCollider meshCollider;

    [NonSerialized] List<Vector3> verticies;
    [NonSerialized] List<int> triangles;

    public bool useTerrainTypes;
    [NonSerialized] List<Vector3> terrainTypes;

    private void Awake()
    {
        GetComponent<MeshFilter>().mesh = squareMesh = new Mesh();
        meshCollider = gameObject.AddComponent<MeshCollider>();
        squareMesh.name = "Square Mesh";
    }

    public void Clear() 
    {
        squareMesh.Clear();
        if (useTerrainTypes) {
            terrainTypes = ListPool<Vector3>.Get();
        }
        verticies = ListPool<Vector3>.Get();
        triangles = ListPool<int>.Get();
    }

    public void Apply()
    {
        squareMesh.SetVertices(verticies);
        ListPool<Vector3>.Add(verticies);

        if (useTerrainTypes)
        {
            squareMesh.SetUVs(2, terrainTypes);
            ListPool<Vector3>.Add(terrainTypes);
        }

        squareMesh.SetTriangles(triangles, 0);
        ListPool<int>.Add(triangles);

        squareMesh.RecalculateNormals();
        meshCollider.sharedMesh = squareMesh;
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

    public void AddTriangleTerrainTypes(Vector3 types) 
    {
        terrainTypes.Add(types);
        terrainTypes.Add(types);
        terrainTypes.Add(types);
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

    public void AddQuadTerrainTypes(Vector3 types) 
    {
        terrainTypes.Add(types);
        terrainTypes.Add(types);
        terrainTypes.Add(types);
        terrainTypes.Add(types);
    }



}
