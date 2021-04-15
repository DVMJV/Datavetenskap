using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SquareCoordinates
{
    [SerializeField]
    private int x, z;

    public int X { get { return x; }}
    public int Z { get { return z; }}
    public int Y { get { return -x - z; }}

    public SquareCoordinates(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public static SquareCoordinates FromOffsetCoordinates(int x, int z)
    {
        return new SquareCoordinates(x, z);
    }

    public override string ToString()
    {
        return "(" + X.ToString() + ", " +Y.ToString() + ", " + Z.ToString() + ")";
    }
    public string ToStringOnSeparateLines() 
    {
        return X.ToString() + "\n" + Y.ToString() + "\n" + Z.ToString();
    }

}
