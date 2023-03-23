using UnityEngine;

public class CellController : MonoBehaviour
{
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

    public CellPool Pool
    {
        set => sc.Pool = value;
    }

    [SerializeField] SensorController sensor;
    public SensorController Sensors => sensor;

    [SerializeField] NeuralNetworkController neuralNetworkController;
    public NeuralNetwork NeuralNetwork
    {
        get => neuralNetworkController.neuralNetwork;
        set => neuralNetworkController.neuralNetwork = value;
    }
}