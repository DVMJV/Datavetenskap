using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TileMap))]
public class TileMapMouse : MonoBehaviour
{
    public Transform selectionCube;
    TileMap tileMap;
    Collider coll;
    Vector3 tilePosHit;
    Vector3 currentTileCoord;
  

    private void Start()
    {
        coll = GetComponent<Collider>();
        tileMap = GetComponent<TileMap>();
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (coll.Raycast(ray, out hitInfo, Mathf.Infinity))
        {
            // Returns correct local position regardless of mesh position and rotation.
            tilePosHit = transform.worldToLocalMatrix.MultiplyPoint3x4(hitInfo.point);
            int x = Mathf.FloorToInt(tilePosHit.x / tileMap.tileSize);
            int z = Mathf.FloorToInt(tilePosHit.z / tileMap.tileSize);
            currentTileCoord.x = x;
            currentTileCoord.z = z;
            // currentTileCoord.y  // Height data from tilemap.

            selectionCube.GetComponent<MeshRenderer>().enabled = true;
            selectionCube.transform.position = currentTileCoord * tileMap.tileSize;
            selectionCube.transform.position += new Vector3(tileMap.tileSize / 2, 0, tileMap.tileSize / 2);                                           
        }
        else
        {
            selectionCube.GetComponent<MeshRenderer>().enabled = false;
        }
        if (Input.GetMouseButtonDown(0)) // && target != null.
        {
            if(selectionCube.GetComponent<MeshRenderer>().enabled)
            {

                Vector3 pos = new Vector3(currentTileCoord.x + tileMap.tileSize / 2, currentTileCoord.y, currentTileCoord.z + tileMap.tileSize / 2);
                EventHandler.current.TileSelected(pos);
            }
        }
    }
}
