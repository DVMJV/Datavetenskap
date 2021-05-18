using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquareCell : MonoBehaviour
{
    [SerializeField]
    SquareCell[] neighbors;

    public SquareGridChunk chunk;
    public RectTransform uiRect;

    public bool obstructed = false;

    public SquareCell PathFrom { get; set; }

    int distance;
    public int SearchHeuristic { get; set; }

    public int SearchPriority
    {
        get { return distance + SearchHeuristic; }
        set { SearchPriority = value; }
    }
    public SquareCell NextWithSamePriority { get; set; }

    public int Distance
    {
        get { return distance; }
        set
        {
            distance = value;
            UpdateDistanceLabel();
        }
    }

    public int SearchPhase { get; set; }

    public SquareCoordinates coordinates;

    float terrainTypeIndex;
    public enum TYPE {WATER, BEACH, FOREST, ELECTRIC, METAL}; // todo use this..
    public TYPE biomeType = TYPE.WATER;

    public float TerrainTypeIndex
    {
        get { return terrainTypeIndex; }
        set
        {
            if (terrainTypeIndex == value)
                return;

            terrainTypeIndex = value;
            Refresh();
        }
    }
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
    int elevation = 0;

    public SquareCell GetNeighbor(SquareDirection direction) 
    {
        return neighbors[(int)direction];
    }

    void UpdateDistanceLabel()
    {
        Text label = uiRect.GetComponent<Text>();
        label.text = distance == int.MaxValue ? "" : distance.ToString();
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

    public void EnableHighlight(Color color)
    {
        Image highlight = uiRect.GetChild(0).GetComponent<Image>();
        highlight.color = color;
        highlight.enabled = true;
    }
    public void DisableHighlight()
    {
        Image highlight = uiRect.GetChild(0).GetComponent<Image>();
        highlight.enabled = false;
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
