using UnityEngine;

public class Spawner
{
    readonly CellPool pool;
    readonly Transform container;

    public Spawner(CellPool pool, Transform container)
    {
        this.pool = pool;
        this.container = container;
    }

    public CellController Spawn(Vector2 position)
    {
        var cell = pool.Get();
        cell.transform.SetPositionAndRotation(position, Quaternion.identity);
        cell.transform.parent = container;
        return cell;
    }
}