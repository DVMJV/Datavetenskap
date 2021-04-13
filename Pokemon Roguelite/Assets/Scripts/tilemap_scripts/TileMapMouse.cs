using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapMouse : MonoBehaviour
{
    public Color highlightColor;
    Color defaultColor;
    Collider coll;
    Renderer rend;

    private void Start()
    {
        coll = GetComponent<Collider>();
        rend = GetComponent<Renderer>();
        defaultColor = rend.material.color;
        highlightColor = Color.cyan;
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (coll.Raycast(ray, out hitInfo, Mathf.Infinity))
        {
            // Returns correct local position regardless of mesh position and rotation.
            Debug.Log(transform.worldToLocalMatrix.MultiplyPoint3x4(hitInfo.point));
        }
        else
        {
           // rend.material.color = defaultColor;
        }
    }
}
