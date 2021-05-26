using System.Collections.Generic;
using UnityEngine;

public class SquareMapGenerator : MonoBehaviour
{

    public SquareGrid grid;
    int cellCount;

    SquareCellPriorityQueue prioQueue;
    int searchFrontierPhase;

    [Range(0f, 0.5f)]
    public float jitterProbability = 0.25f;

    [Range(0, 100)]
    public int chunkSizeMin = 10;

    [Range(20, 100)]
    public int chunkSizeMax = 40;

    [Range(5, 95)]
    public int landPercentage = 50;

    float offsetX;
    float offsetZ;

    public List<GameObject> beachObstructible;
    public List<GameObject> beachNonObstructible;

    public List<GameObject> metalObstructible;
    public List<GameObject> metalNonObstructible;

    public List<GameObject> electricObstructible;
    public List<GameObject> electricNonObstructible;

    public List<GameObject> forestObstructible;
    public List<GameObject> forestNonObstructible;

    public GameObject waterPrefab;

    // obstructionprob, faunaprob.
    //[Range(0, 100)]
    //int probabilityLevel = 35;
    [Range(0, 100)]
    public int forestObstruct;
    [Range(0, 100)]
    public int forestNonObstruct;

    [Range(0, 100)]
    public int metalObstruct;
    [Range(0, 100)]
    public int metalNonObstruct;

    [Range(0, 100)]
    public int electricObstruct;
    [Range(0, 100)]
    public int electricNonObstruct;

    [Range(0, 100)]
    public int beachObstruct;
    [Range(0, 100)]
    public int beachNonObstruct;

    List<SquareCell> spawnCells = new List<SquareCell>();

    public void GenerateMap(int x, int z)
    {
        cellCount = x * z;
        grid.CreateMap(x, z);

        if (prioQueue == null)
            prioQueue = new SquareCellPriorityQueue();

        CreateLand();
        SetTerrainType();

        for (int i = 0; i < cellCount; i++)
        {
            SquareCell cell = grid.GetCell(i);
            cell.SearchPhase = 0;
            bool neighborObstructed = false;

            for (SquareDirection d = SquareDirection.UP; d <= SquareDirection.LEFT; d++)
            {
                SquareCell neighbor = cell.GetNeighbor(d);
                if(neighbor == null)
                    continue;
                if (neighbor.obstructed)
                    neighborObstructed = true;

                SquareCell neighborNeighbor;

                if (d == SquareDirection.UP || d == SquareDirection.DOWN)
                {
                    neighborNeighbor = neighbor.GetNeighbor(SquareDirection.LEFT);
                    if (neighborNeighbor != null && neighborNeighbor.obstructed)
                        neighborObstructed = true;
                    neighborNeighbor = neighbor.GetNeighbor(SquareDirection.RIGHT);
                    if (neighborNeighbor != null && neighborNeighbor.obstructed)
                        neighborObstructed = true;
                }
                else
                {
                    neighborNeighbor = neighbor.GetNeighbor(SquareDirection.DOWN);
                    if (neighborNeighbor != null && neighborNeighbor.obstructed)
                        neighborObstructed = true;
                    neighborNeighbor = neighbor.GetNeighbor(SquareDirection.UP);
                    if (neighborNeighbor != null && neighborNeighbor.obstructed)
                        neighborObstructed = true;

                }
            }
            

            if (cell.biomeType == SquareCell.TYPE.WATER) // Water
            {
                GameObject item = waterPrefab;
                GameObject go = Instantiate(item);
                go.transform.position = cell.transform.position + new Vector3(0, 5f);
                cell.obstructed = true;
            }


            if (!neighborObstructed)
            {
                offsetX = Random.Range(-3, 3);
            offsetZ = Random.Range(-3, 3);
            int probability = Random.Range(0, 100);
            
            if (cell.biomeType == SquareCell.TYPE.BEACH)
            {
                if (beachObstruct >= probability)
                {
                    GameObject item = beachObstructible[(int)Random.Range(0, beachObstructible.Count)];
                    GameObject go = Instantiate(item);
                    go.transform.position = cell.transform.position;
                    cell.obstructed = true;

                }
                else if (beachNonObstruct >= probability)
                {
                    GameObject item = beachNonObstructible[(int)Random.Range(0, beachNonObstructible.Count)];
                    GameObject go = Instantiate(item);
                    go.transform.position = cell.transform.position;
                }
            }
            if (cell.biomeType == SquareCell.TYPE.FOREST) // gör mer skogig.
            {
                if (forestObstruct >= probability)
                {
                    GameObject item = forestObstructible[(int)Random.Range(0, forestObstructible.Count)];
                    GameObject go = Instantiate(item);
                    go.transform.position = cell.transform.position;
                    cell.obstructed = true;

                }
                else if (forestNonObstruct >= probability)
                {
                    GameObject item = forestNonObstructible[(int)Random.Range(0, forestNonObstructible.Count)];
                    GameObject go = Instantiate(item);
                    go.transform.position = cell.transform.position;
                }
            }
            if (cell.biomeType == SquareCell.TYPE.ELECTRIC)
            {
                if (electricObstruct >= probability)
                {
                    GameObject item = electricObstructible[(int)Random.Range(0, electricObstructible.Count)];
                    GameObject go = Instantiate(item);
                    go.transform.position = cell.transform.position;
                    cell.obstructed = true;

                }
                else if (electricNonObstruct >= probability)
                {
                    GameObject item = electricNonObstructible[(int)Random.Range(0, electricNonObstructible.Count)];
                    GameObject go = Instantiate(item);
                    go.transform.position = cell.transform.position;
                }

            }
            if (cell.biomeType == SquareCell.TYPE.METAL) // gör mer skogig
            {
                if (metalObstruct >= probability)
                {
                    GameObject item = metalObstructible[(int)Random.Range(0, metalObstructible.Count)];
                    GameObject go = Instantiate(item);
                    go.transform.position = cell.transform.position;
                    cell.obstructed = true;
                }
                else if (metalNonObstruct >= probability)
                {
                    GameObject item = metalNonObstructible[(int)Random.Range(0, metalNonObstructible.Count)];
                    GameObject go = Instantiate(item);
                    go.transform.position = cell.transform.position;
                }
            }
            }
            
            //}
            

            if (!cell.obstructed)
            {
                spawnCells.Add(cell);
            }
        }

        EventHandler.current.MapGenerated(grid.GetCells());
        EventHandler.current.PlayerSpawnCells(spawnCells);
    }

    void CreateLand()
    {
        int landBudget = Mathf.RoundToInt(cellCount * landPercentage * 0.01f);
        while (landBudget > 0)
        {
            landBudget = RaiseTerrain(Random.Range(chunkSizeMin, chunkSizeMax + 1), landBudget);
        }
    }

    int RaiseTerrain(int chunkSize, int budget)
    {
        searchFrontierPhase += 1;
        SquareCell firstCell = GetRandomCell();
        firstCell.SearchPhase = searchFrontierPhase;
        firstCell.Distance = 0;
        firstCell.SearchHeuristic = 0;
        prioQueue.Enqueue(firstCell);
        SquareCoordinates center = firstCell.coordinates;

        int size = 0;
        while (size < chunkSize && prioQueue.Count > 0)
        {
            SquareCell current = prioQueue.Dequeue();
            if (current.TerrainTypeIndex == 0) // 0 is water, or will be..
            {
                current.TerrainTypeIndex = 1; // randomize?
                if (--budget == 0)
                    break;
            }
            current.Elevation++;
            size++;

            for (SquareDirection d = SquareDirection.UP; d <= SquareDirection.LEFT; d++)
            {
                SquareCell neighbor = current.GetNeighbor(d);
                if (neighbor && neighbor.SearchPhase < searchFrontierPhase)
                {
                    neighbor.SearchPhase = searchFrontierPhase;
                    neighbor.Distance = neighbor.coordinates.DistanceTo(center);
                    neighbor.SearchHeuristic = Random.value < jitterProbability ? 1 : 0;
                    prioQueue.Enqueue(neighbor);
                }
            }
        }
        prioQueue.Clear();
        return budget;
    }

    void SetTerrainType()
    {
        for (int i = 0; i < cellCount; i++)
        {
            SquareCell cell = grid.GetCell(i);

            // Get right type on each tile. Clamp to last type.
            cell.TerrainTypeIndex = cell.Elevation;
            cell.biomeType = (SquareCell.TYPE)((int)cell.biomeType + cell.TerrainTypeIndex);

            if ((int)cell.biomeType > (int)SquareCell.TYPE.METAL)
                cell.biomeType = SquareCell.TYPE.METAL;
            /*
            //Sqush map elevation.
            if (cell.Elevation != 0)
                cell.Elevation = 1;
            else
                cell.Elevation = -3; // Move water down.
                */
            if (cell.Elevation == 0)
                cell.Elevation = -3;
        }

    }

    SquareCell GetRandomCell()
    {
        return grid.GetCell(Random.Range(0, grid.cellCountX * grid.cellCountZ));

    }
}
