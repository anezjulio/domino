using UnityEngine;

public class TileData
{
    public int valueA;
    public int valueB;

    public TileData(int a, int b)
    {
        valueA = a;
        valueB = b;
    }

    public override string ToString()
    {
        return $"[{valueA}|{valueB}]";
    }
}
