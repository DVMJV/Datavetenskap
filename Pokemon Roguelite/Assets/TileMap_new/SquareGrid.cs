using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
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

    List<SquareCell> walkableTiles = new List<SquareCell>();

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

    private void ClearHighlights()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].Distance = int.MaxValue;
            cells[i].DisableHighlight();
        }
    }

    private void Start()
    {
        EventHandler.current.onAllySelected += FindAllPossibleTiles;
        EventHandler.current.onTurnEnd += ClearHighlights;
        EventHandler.current.onMovePokemon += FindPath;
        EventHandler.current.clearHighlights += ClearHighlights;
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
        if (chunks != null)
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

    public void FindAllPossibleTiles(PokemonContainer selectedPokemon)
    {
        if (selectedPokemon == null)
        {
            ClearHighlights();
            return;
        }
        SearchForTiles(selectedPokemon.currentMovement, selectedPokemon.CurrentTile);
    }

    public void FindPath(SquareCell fromCell, PokemonContainer pokemon)
    {
        SearchForPath(fromCell, pokemon);
    }

    void SearchForTiles(int speed, SquareCell currentTile)
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].Distance = int.MaxValue;
            cells[i].DisableHighlight();
            walkableTiles.Clear();
        }

        Queue<SquareCell> openSet = new Queue<SquareCell>();
        currentTile.Distance = 0;
        openSet.Enqueue(currentTile);

        while (openSet.Count > 0)
        {
            SquareCell current = openSet.Dequeue();

            if (current.Distance >= speed && current.Distance != int.MaxValue)
                continue;

            for (SquareDirection d = SquareDirection.UP; d <= SquareDirection.LEFT; d++)
            {
                SquareCell neighbor = current.GetNeighbor(d);

                if (neighbor == null || Mathf.Abs(current.Elevation - neighbor.Elevation) > 2 || neighbor.obstructed)
                    continue;
                else if (neighbor.Distance == int.MaxValue)
                {
                    neighbor.Distance = current.Distance + 1;
                    if (neighbor.Distance > speed)
                        continue;
                    else
                    {
                        neighbor.EnableHighlight(Color.blue);
                        openSet.Enqueue(neighbor);
                        walkableTiles.Add(neighbor);
                    }
                }
            }
        }
    }

    void SearchForPath(SquareCell toCell, PokemonContainer pokemon)
    {
        SquareCell fromCell = pokemon.CurrentTile;
        int speed = pokemon.currentMovement;

        if (!walkableTiles.Contains(toCell))
            return;

        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].Distance = int.MaxValue;
        }

        fromCell.Distance = 0;

        SquareCellPriorityQueue openSet = new SquareCellPriorityQueue();
        openSet.Enqueue(fromCell);

        while (openSet.Count > 0)
        {
            SquareCell current = openSet.Dequeue();

            if (current == toCell)
            {
                ConstructPath(toCell, pokemon);
                break;
            }

            for (SquareDirection d = SquareDirection.UP; d <= SquareDirection.LEFT; d++)
            {
                SquareCell neighbor = current.GetNeighbor(d);
                if (neighbor == null || Mathf.Abs(current.Elevation - neighbor.Elevation) > 2 ||
                    !walkableTiles.Contains(neighbor) || neighbor.obstructed)
                    continue;

                else if (neighbor.Distance == int.MaxValue)
                {
                    neighbor.Distance = current.Distance + 1;
                    neighbor.SearchHeuristic = neighbor.coordinates.DistanceTo(toCell.coordinates);
                    neighbor.PathFrom = current;
                    openSet.Enqueue(neighbor);
                }
                else if (current.Distance + 1 < neighbor.Distance)
                {
                    int oldPriority = neighbor.SearchPriority;
                    neighbor.Distance = current.Distance + 1;
                    neighbor.PathFrom = current;
                    openSet.Change(neighbor, oldPriority);
                }
            }
        }
    }


    void ConstructPath(SquareCell toCell, PokemonContainer pokemon)
    {
        Stack<SquareCell> stack = new Stack<SquareCell>();
        while (toCell != pokemon.CurrentTile)
        {
            stack.Push(toCell);
            toCell = toCell.PathFrom;
        }

        EventHandler.current.PathFound(stack, pokemon);
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