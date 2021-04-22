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
            EditCell(squareGrid.GetCell(hit.point, activeColor));
        }
    }

    void EditCell(SquareCell cell)
    {
        cell.Color = activeColor;
        cell.Elevation = activeElevation;
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