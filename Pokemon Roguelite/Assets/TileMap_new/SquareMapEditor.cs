using UnityEngine;

public class SquareMapEditor : MonoBehaviour
{
    public Color[] colors;
    public SquareGrid squareGrid;
    private Color activeColor;

    SquareCell searchFromCell, searchToCell;

    int activeElevation;

    private void Awake()
    {
        SelectColor(0);
    }

    private void Update()
    {
        HandleInput();
        
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

                if (Input.GetKey(KeyCode.LeftShift) && searchToCell != currentCell)
                {
                    if (searchFromCell)
                        searchFromCell.DisableHighlight();
                    searchFromCell = currentCell;
                    searchFromCell.EnableHighlight(Color.blue);

                    if (searchToCell)
                    {
                        squareGrid.FindPath(searchFromCell, searchToCell);
                    }
                }
            }
            
            if(searchFromCell && searchFromCell != currentCell)
            {
                searchToCell = currentCell;
                squareGrid.FindPath(searchFromCell, searchToCell);
            }
        }
    }

    void EditCell(SquareCell cell)
    {
        cell.color = activeColor;
        cell.Elevation = activeElevation;
      //  Debug.Log(activeElevation);
        squareGrid.Refresh();
    }

    public void SelectColor(int index) 
    {
        activeColor = colors[index];
    }

    public void SetElevation(float elevation)
    {
        activeElevation = (int)elevation;
    }


}