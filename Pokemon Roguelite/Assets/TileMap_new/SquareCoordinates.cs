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

    public static SquareCoordinates FromPosition(Vector3 position)
    {
        float x = position.x / SquareMetrics.height;
        float y = -x;

        float offset = position.z / SquareMetrics.width;
        x -= offset;
        y -= offset;

        int iX = Mathf.RoundToInt(x);
        int iY = Mathf.RoundToInt(y);
        int iZ = Mathf.RoundToInt(-x - y);

        if (iX + iY + iZ != 0)
        {
            float dX = Mathf.Abs(x - iX);
            float dY = Mathf.Abs(y - iY);
            float dZ = Mathf.Abs(-x - y - iZ);

            if (dX > dY && dX > dZ)
            {
                iX = -iY - iZ;
            }
            else if (dZ > dY)
            {
                iZ = -iX - iY;
            }
        }
        Debug.Log(iX + ", " + iZ);
        return new SquareCoordinates(iX, iZ);
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
