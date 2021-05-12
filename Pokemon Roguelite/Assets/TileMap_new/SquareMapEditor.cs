using UnityEngine;

public class SquareMapEditor : MonoBehaviour
{
    public SquareGrid squareGrid;

    SquareCell searchFromCell, searchToCell;

    int activeElevation;

    public float[] terrainTypeIndexes;
    float activeTerrainIndex;

    private void Awake()
    {
        SelectTerrainIndex(0);
    }

    private void Update()
    {
        HandleInput();
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

            if(Input.GetMouseButtonDown(0))
            {
                EventHandler.current.TileSelected(currentCell);
                EditCell(currentCell);
            }
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