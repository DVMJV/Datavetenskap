using UnityEngine;

public class SquareMapEditor : MonoBehaviour
{
    public Color[] colors;
    public SquareGrid squareGrid;
    private Color activeColor;

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
            SquareCell currentCell = squareGrid.GetCell(hit.point, activeColor);

            if(Input.GetMouseButtonDown(0))
            {
                EventHandler.current.TileSelected(currentCell);
                EditCell(currentCell);
            }
            EditCell(squareGrid.GetCell(hit.point));
        }
    }

    void EditCell(SquareCell cell)
    {
        cell.TerrainTypeIndex = activeTerrainIndex;
        cell.Elevation = activeElevation;
        squareGrid.Refresh();
    }

    public void SelectTerrainIndex(float index)
    {
        activeTerrainIndex = terrainTypeIndexes[(int)index];
    }

    public void SetElevation(float elevation)
    {
        activeElevation = (int)elevation;
      //  Debug.Log(activeElevation);
    }


}