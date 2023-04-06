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

    void Update()
    {
        if (transform.position.magnitude > worldData.Fence * sc.Size)
            pool.Deactivate(this);
    }
}