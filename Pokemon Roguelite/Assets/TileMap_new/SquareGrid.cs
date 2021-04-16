using UnityEngine;
using UnityEngine.UI;

public class SquareGrid : MonoBehaviour
{
    public int width = 5;
    public int height = 5;

    public SquareCell cellPrefab;
    SquareCell[] cells;

    public Text cellLabelPrefab;
    Canvas gridCanvas;

    SquareMesh squareMesh;
    MeshCollider meshCollider;

    public Color defaultColor = Color.white;

    private void Awake()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
        squareMesh = GetComponentInChildren<SquareMesh>();

        cells = new SquareCell[height * width];
        for (int z = 0, i = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                CreateCell(x, z, i++);
            }
        }
    }

    private void Start()
    {
        squareMesh.Triangulate(cells);
    }

    public void TouchCell(Vector3 position, Color color) 
    {
        position = transform.worldToLocalMatrix.MultiplyPoint3x4(position); // Bugfix.
        SquareCoordinates coordinates = SquareCoordinates.FromPosition(position);

        int index = ((coordinates.X + (coordinates.Z * width)));
        SquareCell cell = cells[index];
        cell.color = color;
        Debug.Log("Touch cell color: " + color);
        squareMesh.Triangulate(cells);

      //  Debug.Log("Hit: " + coordinates.ToString());
    }

    void CreateCell(int x, int z, int i)
    {
        Vector3 position;
        position.x = x * 10f;
        position.y = 0;
        position.z = z * 10f;

        SquareCell cell = cells[i] = Instantiate<SquareCell>(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = SquareCoordinates.FromOffsetCoordinates(x, z); // Create struct with coordinates. Might need adjustment.

        cell.color = defaultColor;


        Text label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToStringOnSeparateLines();



    }
}


