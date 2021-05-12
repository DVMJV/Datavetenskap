using UnityEngine;
using UnityEngine.UI;

public class SquareGrid : MonoBehaviour
{
    public int cellCountX = 20, cellCountZ = 15;
    int chunkCountX, chunkCountZ;

    public SquareCell cellPrefab;
    SquareCell[] cells;

    public Text cellLabelPrefab;

    public SquareGridChunk chunkPrefab;
    SquareGridChunk[] chunks;

    MeshCollider meshCollider;

    public float defaultTerrainIndex = 0;

    private void Awake()
    {
        CreateMap(cellCountX, cellCountZ);
    }

    public void ShowUI(bool visible) 
    {
        for (int i = 0; i < chunks.Length; i++)
            chunks[i].ShowUI(visible);
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

    public void CreateMap(int x, int z)
    {
        // Clear old data
        if (chunks!=null)
            for (int i = 0; i < chunks.Length; i++)
                Destroy(chunks[i].gameObject);

        cellCountX = x;
        cellCountZ = z;
        chunkCountX = cellCountX / SquareMetrics.chunkSizeX;
        chunkCountZ = cellCountZ / SquareMetrics.chunkSizeZ;
        CreateChunks();
        CreateCells();
    }

    // Different ways to get index.
    public SquareCell GetCell(Vector3 position) 
    {
        position = transform.worldToLocalMatrix.MultiplyPoint3x4(position); // Bugfix.
        SquareCoordinates coordinates = SquareCoordinates.FromPosition(position);
        int index = ((coordinates.X + (coordinates.Z * cellCountX)));
       // Debug.Log("Hit: " + coordinates.ToString());
        return cells[index];     
    }

    public SquareCell GetCell(int xOffset, int zOffset)
    {
        return cells[xOffset + zOffset * cellCountX];
    }

    public SquareCell GetCell(int cellIndex)
    {
        return cells[cellIndex];
    }

    void CreateCell(int x, int z, int i)
    {
        Vector3 position;
        position.x = x * 10f;
        position.y = 0;
        position.z = z * 10f;

        SquareCell cell = cells[i] = Instantiate<SquareCell>(cellPrefab);
        cell.transform.localPosition = position;
        cell.coordinates = SquareCoordinates.FromOffsetCoordinates(x, z);
       // cell.TerrainTypeIndex = Random.Range(0, 4); // Use to generate noise to test terrainTypes.

        if (x > 0)
            cell.SetNeighbor(SquareDirection.LEFT, cells[i - 1]);
        if (z > 0)
        {
            cell.SetNeighbor(SquareDirection.DOWN, cells[i - cellCountX]);
        }

        Text label = Instantiate<Text>(cellLabelPrefab);
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
        int localZ = z - chunkZ * SquareMetrics.chunkSizeZ;
        chunk.AddCell(localX + localZ * SquareMetrics.chunkSizeX, cell);
    }
}


