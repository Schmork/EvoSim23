using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CellPool : MonoBehaviour
{
    GameObject _prefab;
    readonly Queue<CellController> _inactiveCells = new();
    readonly HashSet<CellController> _activeCells = new();

    public int NumActives => _activeCells.Count;
    public CellController[] Actives => _activeCells.ToArray();

    public void SetPrefab(GameObject prefab) => _prefab = prefab;

    public CellController Get()
    {
        //Debug.Log("active: " + NumActives() + ", inactive: " + _inactiveCells.Count);
        CellController cell;
        if (_inactiveCells.Count > 0)
        {
            cell = _inactiveCells.Dequeue();
        }
        else
        {
            cell = Instantiate(_prefab).GetComponent<CellController>();
        }

        cell.gameObject.SetActive(true);
        _activeCells.Add(cell);
        return cell;
    }

    public void Deactivate(CellController cell)
    {
        _activeCells.Remove(cell);
        _inactiveCells.Enqueue(cell);
        cell.gameObject.SetActive(false);
    }
}