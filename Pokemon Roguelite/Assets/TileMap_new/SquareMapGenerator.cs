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

    // sub lists obstructable, non obstructable.
    public List<GameObject> beachBiome;
    public List<GameObject> metalBiome;
    public List<GameObject> electricBiome;
    public List<GameObject> forestBiome;
    public GameObject waterPrefab;

    // obstructionprob, faunaprob.
    [Range(0, 100)]
    int probabilityLevel = 35;
    float offsetX;
    float offsetZ;

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

            offsetX = Random.Range(-3, 3);
            offsetZ = Random.Range(-3, 3);
            int probability = Random.Range(0, 100);
           
            if (cell.biomeType == SquareCell.TYPE.WATER) // Water
            {           
                GameObject item = waterPrefab;
                item.transform.position = cell.transform.position + new Vector3(0, 5f);
                Instantiate(item);
                cell.obstructed = true;
            }

            if (probabilityLevel >= probability)
            {
                if (cell.biomeType == SquareCell.TYPE.BEACH)
                {
                    GameObject item = beachBiome[(int)Random.Range(0, beachBiome.Count)];
                    item.transform.position = cell.transform.position;
                    Instantiate(item);
                }
                if (cell.biomeType == SquareCell.TYPE.FOREST) // gör mer skogig.
                {
                    int value = (int)Random.Range(0, forestBiome.Count);
                    GameObject item = forestBiome[(int)Random.Range(0, forestBiome.Count)];
                    item.transform.position = cell.transform.position + new Vector3(offsetX, 0, offsetZ);
                    Instantiate(item);
                }
                if (cell.biomeType == SquareCell.TYPE.ELECTRIC)
                {
                    GameObject item = electricBiome[(int)Random.Range(0, electricBiome.Count)];
                    item.transform.position = cell.transform.position + new Vector3(offsetX, 0, offsetZ);
                    Instantiate(item);
                }
                if (cell.biomeType == SquareCell.TYPE.METAL) // gör mer skogig
                {
                    GameObject item = metalBiome[(int)Random.Range(0, metalBiome.Count)];
                    item.transform.position = cell.transform.position + new Vector3(offsetX, 0, offsetZ);
                    Instantiate(item);
                }
            }
        }
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
                    neighbor.SearchHeuristic = Random.value < jitterProbability ? 1: 0;
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

            //Sqush map elevation.
            if (cell.Elevation != 0)
                cell.Elevation = 1;
            else
                cell.Elevation = -3; // Move water down.
        }
    
    }

    SquareCell GetRandomCell() 
    {
        return grid.GetCell(Random.Range(0, grid.cellCountX * grid.cellCountZ));
    
    }
}
