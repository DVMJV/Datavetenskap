using UnityEngine;
using UnityEngine.UI;

public class SquareGrid : MonoBehaviour
{
    int cellCountX, cellCountZ;
    public int chunkCountX = 4, chunkCountZ = 3;

    public SquareCell cellPrefab;
    SquareCell[] cells;

    public Text cellLabelPrefab;
    //Canvas gridCanvas;

    public SquareGridChunk chunkPrefab;
    SquareGridChunk[] chunks;

    //SquareMesh squareMesh;
    MeshCollider meshCollider;

    public Color defaultColor = Color.white;

    private void Awake()
    {
        //gridCanvas = GetComponentInChildren<Canvas>();
        //squareMesh = GetComponentInChildren<SquareMesh>();

        cellCountX = chunkCountX * SquareMetrics.chunkSizeX;
        cellCountZ = chunkCountZ * SquareMetrics.chunkSizeZ;
        CreateChunks();
        CreateCells();
    }

    void CreateCells() 
    {
        cells = new SquareCell[cellCountZ * cellCountX];
        for (int z = 0, i = 0; z < cellCountZ; z++)
        {
            for (int x = 0; x < cellCountX; x++)
            {
                CreateCell(x, z, i++);
            }
        }
    }

    void CreateChunks() 
    {
        chunks = new SquareGridChunk[chunkCountX * chunkCountZ];

        for (int z = 0, i = 0; z < chunkCountZ; z++)
        {
            for (int x = 0; x < chunkCountX; x++)
            {
                SquareGridChunk chunk = chunks[i++] = Instantiate(chunkPrefab);
                chunk.transform.SetParent(transform);
            }
        }
    }

    //private void Start()
    //{
    //    squareMesh.Triangulate(cells);
    //}

    public SquareCell GetCell(Vector3 position, Color color) 
    {
        position = transform.worldToLocalMatrix.MultiplyPoint3x4(position); // Bugfix.
        SquareCoordinates coordinates = SquareCoordinates.FromPosition(position);
        int index = ((coordinates.X + (coordinates.Z * cellCountX)));
        Debug.Log("Hit: " + coordinates.ToString());
        return cells[index];     
    }

    //public void Refresh()
    //{
    //    squareMesh.Triangulate(cells);
    //}

    void CreateCell(int x, int z, int i)
    {
        Vector3 position;
        position.x = x * 10f;
        position.y = 0;
        position.z = z * 10f;

        SquareCell cell = cells[i] = Instantiate<SquareCell>(cellPrefab);
        //cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = SquareCoordinates.FromOffsetCoordinates(x, z); // Create struct with coordinates. Might need adjustment.
        cell.color = defaultColor;

        if (x > 0)
            cell.SetNeighbor(SquareDirection.LEFT, cells[i - 1]);
        if (z > 0)
        {
            cell.SetNeighbor(SquareDirection.DOWN, cells[i - cellCountX]);
        }

        Text label = Instantiate<Text>(cellLabelPrefab);
        //label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToStringOnSeparateLines();
        cell.uiRect = label.rectTransform;

        AddCellToChunk(x, z, cell);
    }

    void AddCellToChunk(int x, int z, SquareCell cell) 
    {
        int chunkX = x / SquareMetrics.chunkSizeX;
        int chunkZ = z / SquareMetrics.chunkSizeZ;
        SquareGridChunk chunk = chunks[chunkX + chunkZ * chunkCountX];
      
        int localX = x - chunkX * SquareMetrics.chunkSizeX;
        int localZ = z - chunkX * SquareMetrics.chunkSizeZ;
        chunk.AddCell(localX + localZ * SquareMetrics.chunkSizeX, cell);
    }
}


