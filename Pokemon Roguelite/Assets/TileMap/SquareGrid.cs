using UnityEngine;
using UnityEngine.UI;

public class SquareGrid : MonoBehaviour
{
    public int width = 6;
    public int height = 6;

    public SquareCell cellPrefab;
    SquareCell[] cells;

    public Text cellLabelPrefab;
    Canvas gridCanvas;

    SquareMesh squareMesh;

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


    void CreateCell(int x, int z, int i)
    {
        Vector3 position;
        position.x = x * 10f;
        position.y = 0;
        position.z = z * 10f;

        SquareCell cell = cells[i] = Instantiate<SquareCell>(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;

        Text label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        label.text = x.ToString() + "\n" + z.ToString();

    }
}


