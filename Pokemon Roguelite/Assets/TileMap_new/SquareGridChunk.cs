using UnityEngine;
using UnityEngine.UI;

public class SquareGridChunk : MonoBehaviour
{
    SquareCell[] cells;
    SquareMesh squareMesh;
    Canvas gridCanvas;

    private void Awake()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
        squareMesh = GetComponentInChildren<SquareMesh>();

        cells = new SquareCell[SquareMetrics.chunkSizeX * SquareMetrics.chunkSizeZ];
    }

    private void Start()
    {
        squareMesh.Triangulate(cells);
    }

    public void AddCell(int index, SquareCell cell) 
    {
        Debug.Log(index);
        cells[index] = cell;
        cell.transform.SetParent(transform, false);
        cell.uiRect.SetParent(gridCanvas.transform, false);
    }
}
