using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SquareMesh : MonoBehaviour
{
    Mesh squareMesh;
    List<Vector3> verticies;
    List<int> triangles;


    private void Awake()
    {
        GetComponent<MeshFilter>().mesh = squareMesh = new Mesh();
        squareMesh.name = "Square Mesh";
        verticies = new List<Vector3>();
        triangles = new List<int>();
    }

    // Loops trough all the cells triangulating them individually.
    // After that is done assign the generated vertices and triangles to the mesh and calculate the normals.
    public void Triangulate(SquareCell[] cells)
    {
        squareMesh.Clear();
        verticies.Clear();
        triangles.Clear();

        for (int i = 0; i < cells.Length; i++)
        {
            Triangulate(cells[i]);
        }

        squareMesh.vertices = verticies.ToArray();
        squareMesh.triangles = triangles.ToArray();
        squareMesh.RecalculateNormals();
    }

    void Triangulate(SquareCell cell) 
    {
        Vector3 center = cell.transform.localPosition;  // Counter clockwise...
        AddTriangle(center + SquareMetrics.corners[0], center + SquareMetrics.corners[1], center + SquareMetrics.corners[2]);
        AddTriangle(center + SquareMetrics.corners[0], center + SquareMetrics.corners[2], center + SquareMetrics.corners[3]);
    }

    void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3) 
    {
        int vertexIndex = verticies.Count;
        verticies.Add(v1);
        verticies.Add(v2);
        verticies.Add(v3);

        triangles.Add(vertexIndex + 0);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
    }
}
