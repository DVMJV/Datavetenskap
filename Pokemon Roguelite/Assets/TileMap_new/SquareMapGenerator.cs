using System.Collections.Generic;
using UnityEngine;

public class SquareMapGenerator : MonoBehaviour
{

    public SquareGrid grid;
    int cellCount;

    SquareCellPriorityQueue prioQueue;
    int searchPhase;

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
        for (int i = 0; i < chunkSize; i++)
        {
            SquareCell cell = GetRandomCell();
            cell.TerrainTypeIndex = 1;
            cell.Elevation = 1;

        }
    }

    SquareCell GetRandomCell() 
    {
        return grid.GetCell(Random.Range(0, grid.cellCountX * grid.cellCountZ));
    
    }
}
