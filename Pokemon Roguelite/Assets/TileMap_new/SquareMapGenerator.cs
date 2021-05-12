using System.Collections.Generic;
using UnityEngine;

public class SquareMapGenerator : MonoBehaviour
{

    public SquareGrid grid;

    public void GenerateMap(int x, int z) 
    {
        grid.CreateMap(x, z);
        for (int i = 0; i < z; i++)
        { 
            grid.GetCell(x / (int)SquareMetrics.width, i ).TerrainTypeIndex = 1;
        }
    }
}
