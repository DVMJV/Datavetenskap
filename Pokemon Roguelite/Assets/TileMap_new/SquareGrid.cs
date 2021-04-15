using UnityEngine;
using UnityEngine.UI;

public class SquareGrid : MonoBehaviour
{
    public int width = 1;
    public int height = 1;

    public SquareCell cellPrefab;
    SquareCell[] cells;

    public Text cellLabelPrefab;
    Canvas gridCanvas;

    SquareMesh squareMesh;
    MeshCollider meshCollider;

    public Color defaultColor = Color.white;
    public Color clickColor = Color.magenta;

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
            TouchCell(hit.point);
        }
    }

    void TouchCell(Vector3 position) 
    {
        position = transform.worldToLocalMatrix.MultiplyPoint3x4(position); // Bugfix.
        SquareCoordinates coordinates = SquareCoordinates.FromPosition(position);

        int index = (coordinates.X + coordinates.Z);
        SquareCell cell = cells[index];
        cell.color = clickColor;
        squareMesh.Triangulate(cells);

        Debug.Log("Hit: " + coordinates.ToString());
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


