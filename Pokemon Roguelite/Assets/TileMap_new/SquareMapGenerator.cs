using System.Collections.Generic;
using UnityEngine;

public class SquareMapGenerator : MonoBehaviour
{

    public SquareGrid grid;
    int cellCount;

    SquareCellPriorityQueue prioQueue;
    int searchFrontierPhase;

    public void GenerateMap(int x, int z) 
    {
        cellCount = x * z;
        grid.CreateMap(x, z);
        
        if (prioQueue == null)
            prioQueue = new SquareCellPriorityQueue();
        
        RaiseTerrain(30);
        for (int i = 0; i < cellCount; i++)
        {
            grid.GetCell(i).SearchPhase = 0;
        }
    }

    void RaiseTerrain(int chunkSize) 
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
            current.TerrainTypeIndex = 1;
            current.Elevation = Random.Range(0, 2);
            size++;

            for (SquareDirection d = SquareDirection.UP; d <= SquareDirection.LEFT; d++)
            {
                SquareCell neighbor = current.GetNeighbor(d);
                if (neighbor && neighbor.SearchPhase < searchFrontierPhase)
                {
                    neighbor.SearchPhase = searchFrontierPhase;
                    neighbor.Distance = neighbor.coordinates.DistanceTo(center);
                    neighbor.SearchHeuristic = Random.value < 0.5f ? 1: 0;
                   // neighbor.Elevation = 2
                    prioQueue.Enqueue(neighbor);
                }
            }
        }
        prioQueue.Clear();
    }

    SquareCell GetRandomCell() 
    {
        return grid.GetCell(Random.Range(0, grid.cellCountX * grid.cellCountZ));
    
    }
}
