using UnityEngine;
using UnityEngine.UI;

public class SquareGridChunk : MonoBehaviour
{
    SquareCell[] cells;
    SquareMesh squareMesh;
    Canvas gridCanvas;
    bool enabled;

    private void Awake()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
        squareMesh = GetComponentInChildren<SquareMesh>();

        cells = new SquareCell[SquareMetrics.chunkSizeX * SquareMetrics.chunkSizeZ];
        ShowUI(false);
    }

    private void LateUpdate()
    {
        squareMesh.Triangulate(cells);
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
}
