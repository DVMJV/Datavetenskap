using UnityEngine;

public class SquareMapEditor : MonoBehaviour
{
    //public Color[] colors;
    //private Color activeColor;

    public SquareGrid squareGrid;
    int activeElevation;

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
            EditCell(squareGrid.GetCell(hit.point)); //activeIndex
        }
    }

    void EditCell(SquareCell cell)
    {
        //cell.Color = Color;
        cell.terrainTypeIndex = activeTerrainIndex;
        Debug.Log(cell.terrainTypeIndex);
        cell.Elevation = activeElevation;
    }

    //public void SelectColor(int index) 
    //{
    //    activeColor = colors[index];
    //}

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