using System.Linq;
using UnityEngine;

public class SpawnBehaviour : MonoBehaviour
{
    [SerializeField] int maxToSpawn;
    [SerializeField] float spawnRadius;
    [SerializeField] float minSpawnInterval = 0.5f;
    [SerializeField] CellPool pool;
    [SerializeField] GameObject _prefab;
    [SerializeField] float spawnSize;
    [SerializeField] float spawnVariance;
    float _timeSinceLastSpawn;
    Spawner spawner;

    private void Awake()
    {
        pool.SetPrefab(_prefab);
        spawner = new Spawner(pool, pool.transform);
    }

    void Update()
    {
        if (pool.NumActives >= maxToSpawn) return;

        _timeSinceLastSpawn += Time.deltaTime;
        if (_timeSinceLastSpawn < minSpawnInterval) return;

        var size = Random.Range(spawnSize - spawnVariance, spawnSize + spawnVariance);
        var pos = GetSpawnPosition(size, 10);
        if (pos == null) return;

        var cell = spawner.Spawn((Vector2)pos);
        _timeSinceLastSpawn = 0f;
        cell.Size = size;
        cell.Pool = pool;
        cell.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));

        float hue = Random.Range(0f, 360f) / 360f;
        float saturation = 0.9f;
        float lightness = 0.8f;
        cell.Renderer.color = Color.HSVToRGB(hue, saturation, lightness);

        cell.NeuralNetwork = NeuralNetwork.NewRandom();
    }

    Vector2? GetSpawnPosition(float size, int attempts)
    {
        if (attempts <= 0) return null;

        var actives = pool.Actives;
        Vector2 pos = Random.insideUnitCircle * spawnRadius;

        return actives.Any(cell => Vector2.Distance(pos, cell.transform.position) * WorldConfig.SpawnMargin
                                   < SizeController.ToScale(size) + SizeController.ToScale(cell.Size))
            ? GetSpawnPosition(size, attempts - 1)
            : pos;
    }
}