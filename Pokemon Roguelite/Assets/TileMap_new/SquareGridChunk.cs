using UnityEngine;
using UnityEngine.UI;

public class SquareGridChunk : MonoBehaviour
{
    SquareCell[] cells;
    public SquareMesh terrain;
    Canvas gridCanvas;

    private void Awake()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
        cells = new SquareCell[SquareMetrics.chunkSizeX * SquareMetrics.chunkSizeZ];
        ShowUI(false);
    }

    private void LateUpdate()
    {
        Triangulate();
        enabled = false;
    }

    public void AddCell(int index, SquareCell cell) 
    {
      //  Debug.Log(index);
        cells[index] = cell;
        cell.chunk = this;
        cell.transform.SetParent(transform, false);
        cell.uiRect.SetParent(gridCanvas.transform, false);
    }

    public void Refresh()
    {
        enabled = true;
    }

    public void ShowUI(bool visible) 
    {
        gridCanvas.gameObject.SetActive(visible);
    }

    public void Triangulate()
    {
        terrain.Clear();
        for (int i = 0; i < cells.Length; i++)
        {
            Triangulate(cells[i]);
        }
        terrain.Apply();
    }

    void Triangulate(SquareCell cell)
    {
        Vector3 center = cell.transform.localPosition;  // Counter clockwise...
        terrain.AddTriangle(center + SquareMetrics.corners[0], center + SquareMetrics.corners[1], center + SquareMetrics.corners[2]);
        terrain.AddTriangle(center + SquareMetrics.corners[0], center + SquareMetrics.corners[2], center + SquareMetrics.corners[3]);
        
        terrain.AddTriangleColor(cell.Color);
        terrain.AddTriangleColor(cell.Color); // Done twice since it is a square.

        for (int i = 0; i < 4; i++)
        {
            SquareCell neighbor = cell.GetNeighbor((SquareDirection)i);

            if (neighbor == null)
            {
                Vector3 elevationDifference = new Vector3(0, (cell.Elevation + 2) * SquareMetrics.elevationStep, 0);
                if (i == 0)
                {
                    terrain.AddQuad(center + SquareMetrics.corners[1],
                            center + SquareMetrics.corners[0],
                            center + SquareMetrics.corners[0] - elevationDifference,
                            center + SquareMetrics.corners[1] - elevationDifference);
                }
                else if (i == 1)
                {
                    terrain.AddQuad(center + SquareMetrics.corners[2],
                            center + SquareMetrics.corners[1],
                            center + SquareMetrics.corners[1] - elevationDifference,
                            center + SquareMetrics.corners[2] - elevationDifference);
                }
                else if (i == 2)
                {
                    terrain.AddQuad(center + SquareMetrics.corners[3],
                            center + SquareMetrics.corners[2],
                            center + SquareMetrics.corners[2] - elevationDifference,
                            center + SquareMetrics.corners[3] - elevationDifference);
                }
                else
                {
                    terrain.AddQuad(center + SquareMetrics.corners[0],
                    center + SquareMetrics.corners[3],
                    center + SquareMetrics.corners[3] - elevationDifference,
                    center + SquareMetrics.corners[0] - elevationDifference);
                }
                terrain.AddQuadColor(cell.Color);
            }
            else if (cell.Elevation - neighbor.Elevation > 0)
            {
                Vector3 neighborCenter = neighbor.transform.localPosition;
                if (i == 0)
                {
                    terrain.AddQuad(center + SquareMetrics.corners[1],
                           center + SquareMetrics.corners[0],
                           neighborCenter + SquareMetrics.corners[3],
                           neighborCenter + SquareMetrics.corners[2]);
                }
                else if (i == 1)
                {
                    terrain.AddQuad(center + SquareMetrics.corners[2],
                           center + SquareMetrics.corners[1],
                           neighborCenter + SquareMetrics.corners[0],
                           neighborCenter + SquareMetrics.corners[3]);
                }
                else if (i == 2)
                {
                    terrain.AddQuad(center + SquareMetrics.corners[3],
                           center + SquareMetrics.corners[2],
                           neighborCenter + SquareMetrics.corners[1],
                           neighborCenter + SquareMetrics.corners[0]);
                }
                else
                {
                    terrain.AddQuad(center + SquareMetrics.corners[0],
                    center + SquareMetrics.corners[3],
                    neighborCenter + SquareMetrics.corners[2],
                    neighborCenter + SquareMetrics.corners[1]);
                }
                terrain.AddQuadColor(cell.Color);
            }
        }
    }
}
