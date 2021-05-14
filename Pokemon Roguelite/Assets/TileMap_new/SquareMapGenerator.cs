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

    public void GenerateMap(int x, int z) 
    {
        cellCount = x * z;
        grid.CreateMap(x, z);
        
        if (prioQueue == null)
            prioQueue = new SquareCellPriorityQueue();


        CreateLand();
        for (int i = 0; i < cellCount; i++)
        {
            grid.GetCell(i).SearchPhase = 0;
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
            //current.Elevation = Random.Range(0, 2); // extra
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

    SquareCell GetRandomCell() 
    {
        return grid.GetCell(Random.Range(0, grid.cellCountX * grid.cellCountZ));
    
    }
}
