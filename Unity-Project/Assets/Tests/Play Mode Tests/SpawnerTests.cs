using NUnit.Framework;
using UnityEngine;

public class SpawnerTest
{
    [Test]
    public void SpawnCell_SpawnCellAtPosition()
    {
        var container = new GameObject();
        var prefab = new GameObject();
        prefab.AddComponent<CellController>();

        var poolObj = new GameObject();
        var pool = poolObj.AddComponent<CellPool>();
        pool.SetPrefab(prefab);

        var spawner = new Spawner(pool, container.transform);

        var pos = Vector3.up;

        var baseLine = Object.FindObjectsOfType<CellController>().Length;
        spawner.Spawn(pos);

        Assert.AreEqual(pos, Object.FindObjectOfType<CellController>().transform.position);
        Assert.AreEqual(1 + baseLine, Object.FindObjectsOfType<CellController>().Length);

        var spawned = Object.FindObjectOfType<CellController>();
        Assert.IsTrue(spawned.transform.parent == container.transform);
    }
}