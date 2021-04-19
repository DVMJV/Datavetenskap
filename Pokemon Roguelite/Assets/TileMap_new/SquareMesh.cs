using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SquareMesh : MonoBehaviour
{
    Mesh squareMesh;
    MeshCollider meshCollider;
    List<Vector3> verticies;
    List<int> triangles;
    List<Color> colors;


    private void Awake()
    {
        GetComponent<MeshFilter>().mesh = squareMesh = new Mesh();
        meshCollider = gameObject.AddComponent<MeshCollider>();

        squareMesh.name = "Square Mesh";
        verticies = new List<Vector3>();
        colors = new List<Color>();
        triangles = new List<int>();
    }

    // Loops trough all the cells triangulating them individually.
    // After that is done assign the generated vertices and triangles to the mesh and calculate the normals.
    public void Triangulate(SquareCell[] cells)
    {
        squareMesh.Clear();
        verticies.Clear();
        colors.Clear();
        triangles.Clear();

        for (int i = 0; i < cells.Length; i++)
        {
            Triangulate(cells[i]);
        }

        squareMesh.vertices = verticies.ToArray();
        squareMesh.colors = colors.ToArray();
        squareMesh.triangles = triangles.ToArray();
        squareMesh.RecalculateNormals();
        meshCollider.sharedMesh= squareMesh;
    }

    void Triangulate(SquareCell cell) 
    {
        Vector3 center = cell.transform.localPosition;  // Counter clockwise...
        AddTriangle(center + SquareMetrics.corners[0], center + SquareMetrics.corners[1], center + SquareMetrics.corners[2]);
        AddTriangle(center + SquareMetrics.corners[0], center + SquareMetrics.corners[2], center + SquareMetrics.corners[3]);
        AddTriangleColor(cell.color);
        AddTriangleColor(cell.color); // Done twice since it is a square.


        for (int i = 0; i < 4; i++)
        {
            SquareCell neighbor = cell.GetNeighbor((SquareDirection)i);
            
            if (neighbor == null)
                continue;

            if (cell.Elevation - neighbor.Elevation > 0)
            {
                Vector3 neighborCenter = neighbor.transform.localPosition;
                if (i == 0)
                {
                    AddQuad(center + SquareMetrics.corners[1],
                           center + SquareMetrics.corners[0],
                           neighborCenter + SquareMetrics.corners[3],
                           neighborCenter + SquareMetrics.corners[2]);
                }
                else if (i == 1)
                {
                    AddQuad(center + SquareMetrics.corners[1],
                           neighborCenter + SquareMetrics.corners[0],
                           neighborCenter + SquareMetrics.corners[3],
                           center + SquareMetrics.corners[2]);
                }
                else if (i == 2)
                {
                    AddQuad(center + SquareMetrics.corners[3],
                           center + SquareMetrics.corners[2],
                           neighborCenter + SquareMetrics.corners[1],
                           neighborCenter + SquareMetrics.corners[0]);
                }
                else
                {
                    AddQuad(center + SquareMetrics.corners[0],
                    center + SquareMetrics.corners[3],
                    neighborCenter + SquareMetrics.corners[2],
                    neighborCenter + SquareMetrics.corners[1]);
                }
                AddQuadColor(cell.color);
            }
        }
    }

    void AddTriangleColor(Color color) 
    {
        colors.Add(color);
        colors.Add(color);
        colors.Add(color);
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

    void AddQuadColor(Color c)
    {
        colors.Add(c);
        colors.Add(c);
        colors.Add(c);
        colors.Add(c);
    }

    void AddQuad(Vector3 topLeft, Vector3 topRight, Vector3 bottomRight, Vector3 bottomLeft) 
    {
        int vertexIndex = verticies.Count;
        verticies.Add(topLeft);
        verticies.Add(topRight);
        verticies.Add(bottomRight);
        verticies.Add(bottomLeft);

        // Tri #1
        triangles.Add(vertexIndex + 0);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 3);

        // #2
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 3);
    }

}
