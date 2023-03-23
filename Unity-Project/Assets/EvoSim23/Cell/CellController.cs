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

    public float sensorPreyDir;
    public float sensorThreatDir;
    public bool sensorHasPrey;
    public bool sensorHasThreat;
}