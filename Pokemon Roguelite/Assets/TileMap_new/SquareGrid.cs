using System.Collections;
using System.Collections.Generic;
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

    public SquareCell GetCell(Vector3 position, Color color) 
    {
        position = transform.worldToLocalMatrix.MultiplyPoint3x4(position); // Bugfix.
        SquareCoordinates coordinates = SquareCoordinates.FromPosition(position);
        int index = ((coordinates.X + (coordinates.Z * width)));
        Debug.Log("Hit: " + coordinates.ToString());
        return cells[index];     
    }

    public void Refresh() 
    {
        squareMesh.Triangulate(cells);
    }

    public void FindDistancesTo(SquareCell cell)
    {
        StopAllCoroutines();
        StartCoroutine(Search(cell));
    }


    IEnumerator Search(SquareCell cell)
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].Distance = int.MaxValue;
        }

        WaitForSeconds delay = new WaitForSeconds(1 / 60f);
        cell.Distance = 0;

        Queue<SquareCell> openSet = new Queue<SquareCell>();
        openSet.Enqueue(cell);


        while(openSet.Count > 0)
        {
            yield return delay;
            SquareCell current = openSet.Dequeue();

            for(SquareDirection d = SquareDirection.UP; d <= SquareDirection.LEFT; d++)
            {
                SquareCell neighbor = current.GetNeighbor(d);
                if(neighbor != null && neighbor.Distance == int.MaxValue)
                {
                    neighbor.Distance = current.Distance + 1;
                    openSet.Enqueue(neighbor);
                }
            }
        }
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

        if (x > 0)
            cell.SetNeighbor(SquareDirection.LEFT, cells[i - 1]);
        if (z > 0)
        {
            cell.SetNeighbor(SquareDirection.DOWN, cells[i - width]);
        }



        Text label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        cell.uiRect = label.rectTransform;
    }
}


