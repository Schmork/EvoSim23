using UnityEngine;

public class CellController : MonoBehaviour
{
    [SerializeField] WorldData worldData;
    public WorldData WorldData => worldData;

    [SerializeField] ValhallaData valhalla;
    public ValhallaData Valhalla => valhalla;

    [SerializeField] StatsController stats;
    public StatsController Stats => stats;

    [SerializeField] SizeController sc;
    public float Size
    {
        get => sc.Size;
        set => sc.Size = value;
    }

    [SerializeField] SpriteRenderer rendrr;
    public SpriteRenderer Renderer => rendrr;

    [SerializeField] Rigidbody2D rb2d;
    public Rigidbody2D Rb => rb2d;

    CellPool pool;
    public CellPool Pool
    {
        get => pool;
        set
        {
            pool = value;
            sc.Pool = value;
        }
    }

    [SerializeField] SensorController sensor;
    public SensorController Sensors => sensor;

    [SerializeField] NeuralNetworkController neuralNetworkController;
    public NeuralNetwork NeuralNetwork
    {
        get => neuralNetworkController.neuralNetwork;
        set => neuralNetworkController.neuralNetwork = value;
    }

    void LateUpdate()
    {
        // Set wrap boundaries
        var area = worldData.Area;
        var width = area.x * area.z * 0.5f;
        var height = area.y * area.z * 0.5f;
        var minBound = new Vector2(-width, -height);
        var maxBound = new Vector2(width, height);

        // Wrap object around screen edges
        if (transform.position.x < minBound.x)
        {
            transform.position = new Vector3(maxBound.x, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > maxBound.x)
        {
            transform.position = new Vector3(minBound.x, transform.position.y, transform.position.z);
        }
        if (transform.position.y < minBound.y)
        {
            transform.position = new Vector3(transform.position.x, maxBound.y, transform.position.z);
        }
        else if (transform.position.y > maxBound.y)
        {
            transform.position = new Vector3(transform.position.x, minBound.y, transform.position.z);
        }


        //if (transform.position.magnitude > worldData.Fence * sc.Size)
        //    pool.Deactivate(this);
    }
}