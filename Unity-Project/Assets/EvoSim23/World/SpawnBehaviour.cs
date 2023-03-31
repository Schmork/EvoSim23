using System.Linq;
using UnityEngine;

public class SpawnBehaviour : MonoBehaviour
{
    [SerializeField] WorldData worldData;
    [SerializeField] ValhallaData valhallaData;

    [SerializeField] int maxToSpawn;
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

        var gauss = Utility.Gauss(worldData.Gauss);
        var cell = spawner.Spawn((Vector3)pos);
        cell.NeuralNetwork = valhallaData.GetHero();
        cell.NeuralNetwork.Mutate(gauss);
        cell.Size = size;
        cell.Pool = pool;
        cell.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));

        float hue = Random.Range(0f, 360f) / 360f;
        float saturation = 0.9f;
        float lightness = 0.8f;
        cell.Renderer.color = Color.HSVToRGB(hue, saturation, lightness);
        _timeSinceLastSpawn = 0f;
    }

    Vector3? GetSpawnPosition(float size, int attempts)
    {
        if (attempts <= 0) return null;
        var area = worldData.Area;
        var width = area.x * area.z * 0.5f;
        var height = area.y * area.z * 0.5f;
        var pos = new Vector3(Random.Range(-width, width),
                              Random.Range(-height, height),
                              transform.position.z);
        
        var actives = pool.Actives;
        return actives.Any(cell => Vector2.Distance(pos, cell.transform.position) * WorldConfig.SpawnMargin
                                   < SizeController.ToScale(size) + SizeController.ToScale(cell.Size))
            ? GetSpawnPosition(size, attempts - 1)
            : pos;
    }
}