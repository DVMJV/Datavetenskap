using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareCell : MonoBehaviour
{
    [SerializeField]
    SquareCell[] neighbors;

    public RectTransform uiRect;

    public SquareCoordinates coordinates;
    public Color color;

    public int Elevation { get { return elevation; } 
        set { elevation = value;
            Vector3 position = transform.localPosition;
            position.y = value * SquareMetrics.elevationStep;
            transform.localPosition = position;

            Vector3 uiPosition = uiRect.localPosition;
            uiPosition.z = elevation * -SquareMetrics.elevationStep;
            uiRect.localPosition = uiPosition;
            } }

    int elevation;
    
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

}