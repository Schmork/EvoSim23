using UnityEngine;

public class SizeController : MonoBehaviour
{
    [SerializeField] CellController cc;
    public CellPool Pool { private get; set; }

    [SerializeField] float size;
    public float Size
    {
        get => size;
        set
        {
            if (value < WorldConfig.DeathBelowSize)
            {
                Pool.Deactivate(cc);
            }
            else
            {
                size = value;
                transform.localScale = Vector3.one * ToScale();
            }
        }
    }

    public float ToScale() => ToScale(size);
    public static float ToScale(float x) => Mathf.Pow(x + 1, 0.5f) - 1;

    void Update()
    {
        var sizeTime = WorldConfig.ShrinkSpeed * Mathf.Pow(size + 1, 3f) * Time.deltaTime;
        var slow = (1 + cc.Rb.velocity.magnitude) * 5000f;
        Size -= sizeTime / slow * Mathf.Abs(cc.Rb.angularVelocity);
    }
}
