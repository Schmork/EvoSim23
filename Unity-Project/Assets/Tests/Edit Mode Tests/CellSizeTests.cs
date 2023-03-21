using NUnit.Framework;
using UnityEngine;

public class CellSizeTests
{
    [Test]
    public void CellSize_Add()
    {
        var cellSize = new CellSize(12);
        cellSize += 4;
        Assert.AreEqual(16, (float)cellSize);
    }

    [Test]
    public void CellSize_Subtract()
    {
        var cellSize = new CellSize(12);
        cellSize -= 4;
        Assert.AreEqual(8, (float)cellSize);
    }

    [Test]
    public void CellSize_Multiply()
    {
        var cellSize = new CellSize(12);
        cellSize *= 4;
        Assert.AreEqual(48, (float)cellSize);
    }

    [Test]
    public void CellSize_Divide()
    {
        var cellSize = new CellSize(12);
        cellSize /= 4;
        Assert.AreEqual(3, (float)cellSize);
    }

    [Test]
    public void CellSize_ToScale()
    {
        var cellSize = new CellSize(4);
        Assert.LessOrEqual(Mathf.Abs(CellSize.ToScale(cellSize) - 3.95f), 0.005);
    }

    [Test]
    public void CellSize_ImplicitFloat()
    {
        var cellSize = new CellSize(12);
        float floatValue = cellSize;
        Assert.AreEqual(12, floatValue);
    }

    [Test]
    public void CellSize_CumulativeMinus()
    {
        var cellSize = new CellSize(12);
        cellSize -= 1;
        Assert.AreEqual(11, (float)cellSize);
        cellSize -= 1;
        Assert.AreEqual(10, (float)cellSize);
        cellSize -= 1;
        Assert.AreEqual(9, (float)cellSize);
        cellSize -= 0.5f;
        Assert.AreEqual(8.5, (float)cellSize);
        cellSize -= 0.5f;
        Assert.AreEqual(8, (float)cellSize);
        cellSize -= 1;
        Assert.AreEqual(7, (float)cellSize);
        cellSize -= 2;
        Assert.AreEqual(5, (float)cellSize);
        cellSize -= 3.1f;
        Assert.LessOrEqual(Mathf.Abs((float)cellSize - 1.9f), 0.00001);
    }
}
