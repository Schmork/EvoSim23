using UnityEngine;

public class CellController : MonoBehaviour
{
    [SerializeField] float shrinkSpeed = 1f;
    Rigidbody2D rb;
    public CellPool Pool { private get; set; }

    private CellSize size;
    public CellSize Size
    {
        get => size;
        set
        {
            if ((float)value < WorldConfig.DeathBelowSize)
            {
                Pool.Deactivate(this);
            }
            else
            {
                size = value;
                transform.localScale = Vector3.one * CellSize.ToScale(size);
            }
        }
    }

    void Awake()
    {
        if (!TryGetComponent(out rb)) rb = gameObject.AddComponent<Rigidbody2D>();
        size = new CellSize(1);
    }

    void Update()
    {
        var sizeTime = shrinkSpeed * Mathf.Pow((float)Size + 1, 1.1f) * Time.deltaTime;
        var slow = (1 + rb.velocity.magnitude);
        var shrink = sizeTime / 1000f;
        shrink += sizeTime / 1500f / slow * Mathf.Abs(rb.angularVelocity);
        shrink += sizeTime / 500f / slow;
        Size -= shrink;
    }
}
