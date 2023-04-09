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
    [SerializeField] Transform container;
    [SerializeField] float spawnSize;
    float _timeSinceLastSpawn;
    Spawner spawner;
    Bounds[] boundsArray;
    float minX, maxX;

    void Awake()
    {
        pool.SetPrefab(_prefab);
        spawner = new Spawner(pool, container);
        
        minX = float.MaxValue; maxX = -float.MaxValue;
        var colliders = GetComponentsInChildren<Collider2D>();
        boundsArray = new Bounds[colliders.Length];
        for (int i = 0; i < colliders.Length; i++)
        {
            boundsArray[i] = colliders[i].bounds;
            var min = boundsArray[i].center.x - boundsArray[i].extents.x;
            if (min < minX) minX = min;
            var max = boundsArray[i].center.x + boundsArray[i].extents.x;
            if (max > maxX) maxX = max;
            colliders[i].enabled = false;
        }
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
        var hue = (pos.Value.x + (maxX - minX) * 0.5f) / (maxX - minX);
        cell.Renderer.color = Color.HSVToRGB(hue, WorldConfig.FixedSatVal, WorldConfig.FixedSatVal);
        _timeSinceLastSpawn = 0f;
    }

    Vector3? GetSpawnPosition(float size, int attempts)
    {
        if (attempts <= 0) return null;

        var bounds = boundsArray[(int)(Random.value * boundsArray.Length)];
        var pos = bounds.center + (Vector3)Random.insideUnitCircle * bounds.extents.x;

        var actives = pool.Actives;
        return actives.Any(cell => Vector2.Distance(pos, cell.transform.position) * WorldConfig.SpawnMargin
                                   < SizeController.ToScale(size) + SizeController.ToScale(cell.Size))
            ? GetSpawnPosition(size, attempts - 1)
            : pos;
    }
}