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

        var pos = GetSpawnPosition(spawnSize, 10);
        if (pos == null) return;
        
        var cell = spawner.Spawn((Vector3)pos);

        //var parent = pool.Actives.OrderBy(go => Vector2.Distance(go.transform.position, (Vector2)pos)).FirstOrDefault();

        cell.NeuralNetwork = valhallaData.GetHero();
        //cell.NeuralNetwork = parent == null ? NeuralNetwork.NewRandom() : parent.NeuralNetwork.Clone() as NeuralNetwork;
        cell.NeuralNetwork.Mutate(Utility.Gauss(worldData.Gauss));
        cell.Size = spawnSize;
        cell.Pool = pool;
        cell.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
        //cell.Renderer.color = Color.HSVToRGB(Random.value, WorldConfig.FixedSatVal, WorldConfig.FixedSatVal);
        var hue = (pos.Value.x + worldData.Area.x * worldData.Area.z * 0.5f) / (worldData.Area.x * worldData.Area.z);
        cell.Renderer.color = Color.HSVToRGB(hue, WorldConfig.FixedSatVal, WorldConfig.FixedSatVal);
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