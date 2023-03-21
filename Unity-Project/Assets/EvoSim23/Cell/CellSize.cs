using UnityEngine;

public class CellSize
{
    float value;
    public static float ToScale(float input) => Mathf.Exp(0.4f * input) - 1;

    public CellSize(float value = 0) => this.value = value;
    public static implicit operator float(CellSize cellSize) => cellSize.value;

    public static CellSize operator +(CellSize a, float b)
    {
        a.value += b;
        return a;
    }

    public static CellSize operator -(CellSize a, float b)
    {
        a.value -= b;
        return a;
    }

    public static CellSize operator *(CellSize a, float b)
    {
        a.value *= b;
        return a;
    }

    public static CellSize operator /(CellSize a, float b)
    {
        a.value /= b;
        return a;
    }

    public override string ToString()
    {
        return value.ToString();
    }
}