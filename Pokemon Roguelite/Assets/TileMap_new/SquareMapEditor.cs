using UnityEngine;

public class SquareMapEditor : MonoBehaviour
{
    public Color[] colors;
    public SquareGrid squareGrid;
    private Color activeColor;
    int activeElevation;

    private void Awake()
    {
        SelectColor(0);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            HandleInput();
        }
    }

    void HandleInput()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            SquareCell currentCell = squareGrid.GetCell(hit.point, activeColor);
            EditCell(currentCell);
            squareGrid.FindDistancesTo(currentCell);
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
        Debug.Log(activeElevation);
    }


}