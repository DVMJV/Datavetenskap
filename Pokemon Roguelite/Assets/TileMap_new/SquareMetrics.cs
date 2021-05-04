using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SquareMetrics
{
    public const float width = 10f;
    public const float height = 10f;
    public const float elevationStep = 2.5f;
    public const int chunkSizeX = 5, chunkSizeZ = 5;

    public static Vector3[] corners =
        {
            new Vector3(-width/2, 0, height/2),       // Upper left corner.
            new Vector3(width/2, 0, height/2),
            new Vector3(width/2, 0, -height/2),
            new Vector3(-width/2, 0, -height/2 )
        };

    //public static Vector3 GetBridge(SquareDirection direction)
    //{
    //    // Redo..
    //    return (corners[(int)direction] + corners[(int)direction + 1]) *
    //        0.5f;
    //}
}

