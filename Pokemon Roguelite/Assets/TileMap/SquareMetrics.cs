using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SquareMetrics
{
    public const float width = 10f;
    public const float height = 10f;


    public static Vector3[] corners =
        {
            new Vector3(-width/2, 0, height/2),       // Upper left corner.
            new Vector3(width/2, 0, height/2),
            new Vector3(width/2, 0, -height/2),
            new Vector3(-width/2, 0, -height/2 )
        };
}

