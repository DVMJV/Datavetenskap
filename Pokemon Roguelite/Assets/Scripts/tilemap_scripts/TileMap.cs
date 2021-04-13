using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Checks if object has the proper components, if not create them.
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))] 
[RequireComponent(typeof(MeshCollider))] 
public class TileMap : MonoBehaviour
{
    int size_x = 100;
    int size_z = 50;
    float tileSize = 1.0f;

    void Start()
    {
        BuildMesh();
    }

    void BuildMesh() 
    {
        int numTiles = size_x * size_z;
        int numTris = numTiles * 2;

        int vsize_x = size_x + 1;
        int vsize_z = size_z + 1;
        int numVerts = vsize_x * vsize_z;

        // Generate the mesh data
        Vector3[] verticies = new Vector3[numVerts];
        Vector3[] normals = new Vector3[numVerts];
        Vector2[] uv = new Vector2[numVerts];           // Texture-Coordinates 
        
        int[] triangles = new int[numTris * 3];         // Counter-clockwise


        // First pass
        for (int z = 0; z < vsize_z; z++)
            for (int x = 0; x < vsize_x; x++)
            {
                verticies[z * vsize_x + x] = new Vector3(x * tileSize, 0, z * tileSize);
                normals[z * vsize_x + x] = Vector3.up;
                uv[z * vsize_x + x] = new Vector2( (float)x / size_x, (float)z / size_z );
            }

        // Second pass
        for (int z = 0; z < size_z; z++)
            for (int x = 0; x < size_x; x++)
            {
                int squareIndex = z * size_x + x;
                int triOffset = squareIndex * 6;
              
                triangles[triOffset + 0] = z * vsize_x + x +           0;
                triangles[triOffset + 1] = z * vsize_x + x + vsize_x + 0;
                triangles[triOffset + 2] = z * vsize_x + x + vsize_x + 1;

                triangles[triOffset + 3] = z * vsize_x + x +           0;
                triangles[triOffset + 4] = z * vsize_x + x + vsize_x + 1;
                triangles[triOffset + 5] = z * vsize_x + x           + 1;
            }


        // Reference
        {
        //verticies[0] = new Vector3(0, 0, 0);
        //verticies[1] = new Vector3(1, 0, 0);
        //verticies[2] = new Vector3(0, 0, -1);
        //verticies[3] = new Vector3(1, 0, -1);

        //// #1
        //triangles[0] = 0;
        //triangles[1] = 3;
        //triangles[2] = 2;  
        
        //// #2
        //triangles[3] = 0;
        //triangles[4] = 1;
        //triangles[5] = 3;

        //normals[0] = Vector3.up;
        //normals[1] = Vector3.up;
        //normals[2] = Vector3.up;
        //normals[3] = Vector3.up;

        //// Order is different since Unity uses bottom-left as (0, 0)
        //uv[0] = new Vector2(0, 1);
        //uv[1] = new Vector2(1, 1);
        //uv[2] = new Vector2(0, 0);
        //uv[3] = new Vector2(1, 0);      
        }


        // Create a new Mesh and populate it with data
        Mesh mesh = new Mesh();
        mesh.vertices = verticies;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uv;

        // Assign our mesh to our filter/renderer/collider
        MeshFilter mesh_filter = GetComponent<MeshFilter>();
        MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
        MeshCollider mesh_collider = GetComponent<MeshCollider>();

        mesh_filter.mesh = mesh;
    }

}
