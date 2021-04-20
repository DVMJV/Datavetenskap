using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SquareDirection {UP, RIGHT, DOWN, LEFT}

public static class SquareDirectionExtensions
{
    public static SquareDirection Opposite(this SquareDirection direction) 
    {
        return (int)direction < 2 ? (direction + 2) : (direction - 2);
    }

    public static SquareDirection Previous(this SquareDirection direction) 
    {
        return direction == SquareDirection.UP ? SquareDirection.LEFT : (direction - 1);    
    }
    public static SquareDirection Next(this SquareDirection direction)
    {
        return direction == SquareDirection.LEFT ? SquareDirection.UP : (direction + 1);
    }

}