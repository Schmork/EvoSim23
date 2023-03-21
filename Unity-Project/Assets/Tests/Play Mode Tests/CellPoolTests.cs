using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class CellPoolTests
{
    CellPool Setup()
    {
        var prefab = new GameObject().AddComponent<CellController>();
        var pool = new GameObject().AddComponent<CellPool>();
        pool.SetPrefab(prefab.gameObject);
        return pool;
    }

    [Test]
    public void CellPool_Get()
    {
        var pool = Setup();
        var a = pool.Get();
        var b = pool.Get();
        var c = pool.Get();
        pool.Deactivate(b);
        var x = pool.Get();
        Assert.AreNotEqual(x, a);
        Assert.AreNotEqual(x, c);
        Assert.AreEqual(x, b);
    }

    [Test]
    public void CellPool_Deactivate()
    {
        var pool = Setup();
        var a = pool.Get();
        var b = pool.Get();
        var c = pool.Get();
        pool.Deactivate(b);
        Assert.AreEqual(2, pool.NumActives);
    }

    [Test]
    public void CellPool_DeactivationProtection()
    {
        var pool = Setup();
        for (int i = 0; i < 20; i++)
        {
            pool.Get();
        }
        for (int i = 0; i < 10; i++)
        {
            var cell = new GameObject().AddComponent<CellController>();
            pool.Deactivate(cell);
            Assert.AreEqual(20, pool.NumActives);
        }
    }

    [Test]
    public void CellPool_DeactivateInactive()
    {
        var pool = Setup();
        var cell = new GameObject().AddComponent<CellController>();
        pool.Deactivate(cell);

        for (int i = 0; i < 10; i++)
        {
            pool.Get();
            Assert.AreEqual(i + 1, pool.NumActives);
        }
        for (int i = 0; i < 15; i++)
        {
            if (pool.NumActives == 0) continue;
            pool.Deactivate(pool.Actives[Random.Range(0, pool.NumActives)]);
        }
        Assert.AreEqual(0, pool.NumActives);
    }

    [Test]
    public void CellPool_CountActive()
    {
        var pool = Setup();
        pool.Get();
        Assert.AreEqual(1, pool.NumActives);
    }

    [Test]
    public void CellPool_GetActives()
    {
        var pool = Setup();
        var a = pool.Get();
        var b = pool.Get();
        var c = pool.Get();
        var array = new CellController[] { a, b, c };
        Assert.AreEqual(array, pool.Actives);
    }

    [Test]
    public void CellPool_Reuse()
    {
        var pool = Setup();
        var a = pool.Get();
        var b = pool.Get();
        var c = pool.Get();
        pool.Deactivate(b);
        var d = pool.Get();
        Assert.AreEqual(b, d);
    }

    [Test]
    public void CellPool_Create100Remove10()
    {
        var pool = Setup();
        for (int i = 0; i < 100; i++)
        {
            pool.Get();
            Assert.AreEqual(i + 1, pool.NumActives);
        }
        var actives = pool.Actives;
        var inactives = new List<CellController>();
        for (int i = 0; i < 100; i += 10)
        {
            inactives.Add(actives[i]);
            pool.Deactivate(actives[i]);
        }
        foreach (var item in inactives)
        {
            Assert.IsTrue(!pool.Actives.Contains(item));
        }
        Assert.AreEqual(90, pool.Actives.Length);
        Assert.AreEqual(90, pool.NumActives);
    }

    [Test]
    public void CellPool_Simulate10kCreationDeactivation()
    {
        var pool = Setup();

        var alive = 0;
        for (int i = 0; i < 20; i++)
        {
            pool.Get();
            alive++;
        }
        Assert.AreEqual(alive, pool.NumActives);

        for (int n = 0; n < 10000; n++)
        {
            if (Random.value < .5)
            {
                pool.Get();
                alive++;
                Assert.AreEqual(alive, pool.NumActives);
            }
            else
            {
                if (pool.NumActives == 0) continue;
                var i = Random.Range(0, pool.NumActives);
                pool.Deactivate(pool.Actives[i]);
                alive--;
                Assert.AreEqual(alive, pool.NumActives);
            }
        }
    }

    [Test]
    public void CellPool_InactivePreferred()
    {
        var pool = Setup();
        for (int i = 0; i < 10; i++)
        {
            pool.Get();
        }
        Assert.AreEqual(10, pool.NumActives);
        for (int i = 0; i < 3; i++)
        {
            var n = Random.Range(0, pool.NumActives);
            pool.Deactivate(pool.Actives[n]);
        }
        Assert.AreEqual(7, pool.NumActives);
        pool.Get();
        pool.Get();
        pool.Get();
        Assert.AreEqual(10, pool.NumActives);
    }
}