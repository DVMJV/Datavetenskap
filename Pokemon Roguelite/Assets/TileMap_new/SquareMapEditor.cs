using UnityEngine;

public class SquareMapEditor : MonoBehaviour
{
    public SquareGrid squareGrid;
    int activeElevation;

    SquareCell searchFromCell, searchToCell;

    public float[] terrainTypeIndexes;
    float activeTerrainIndex;

    private void Awake()
    {
        SelectTerrainIndex(0);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            HandleInput();
        }
    }

    public void ShowUI(bool visible) 
    {
        squareGrid.ShowUI(visible);
    }

    void HandleInput()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            SquareCell currentCell = squareGrid.GetCell(hit.point);
            EventHandler.current.TileSelected(currentCell);
            EditCell(currentCell);
        }
    }

    void EditCell(SquareCell cell)
    {
        cell.TerrainTypeIndex = activeTerrainIndex;
        cell.Elevation = activeElevation;
    }

    public void SelectTerrainIndex(float index)
    {
        activeTerrainIndex = terrainTypeIndexes[(int)index];
    }

    public void SetElevation(float elevation)
    {
        activeElevation = (int)elevation;
    }


}