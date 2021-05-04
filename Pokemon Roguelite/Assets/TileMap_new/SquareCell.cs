using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareCell : MonoBehaviour
{
    [SerializeField]
    SquareCell[] neighbors;

    public SquareGridChunk chunk;

    public RectTransform uiRect;

    public SquareCoordinates coordinates;

    //public Color Color
    //{
    //    get { return color; }
    //    set
    //    {
    //        if (color == value)
    //            return;

    //        color = value;
    //        Refresh();
    //    }
    //}
    //Color color;

    // added
    public float terrainTypeIndex;


    public int Elevation { get { return elevation; } 
        set
        {
            if (elevation == value)
                return;

            elevation = value;
    
            Vector3 position = transform.localPosition;
            position.y = value * SquareMetrics.elevationStep;
            transform.localPosition = position;

            Vector3 uiPosition = uiRect.localPosition;
            uiPosition.z = elevation * -SquareMetrics.elevationStep;
            uiRect.localPosition = uiPosition;

            Refresh();
            } }
    //int elevation = int.MinValue;
    int elevation = 0;
    
    public SquareCell GetNeighbor(SquareDirection direction) 
    {
        return neighbors[(int)direction];
    }

    public void SetNeighbor(SquareDirection direction, SquareCell cell) 
    {
        neighbors[(int)direction] = cell;         // Set neightbor, also set self as neighbor.
        cell.neighbors[(int)direction.Opposite()] = this;
    }

    public SquareCell[] GetNeighborArray() 
    {
        return neighbors;    
    }

    void Refresh() 
    {
        if (chunk)
        {
            chunk.Refresh();
            for (int i = 0; i < neighbors.Length; i++)
            {
                SquareCell neighbor = neighbors[i];
                if (neighbor != null && neighbor.chunk != chunk)
                    neighbor.chunk.Refresh();
            }
        }
    }
}
