using UnityEngine;

public class SquareMapEditor : MonoBehaviour
{
    public Color[] colors;
    public SquareGrid squareGrid;
    private Color activeColor;

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
            squareGrid.TouchCell(hit.point, activeColor);
        }
    }

    public void SelectColor(int index) 
    {
        activeColor = colors[index];
    }


}