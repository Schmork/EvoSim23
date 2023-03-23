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
            size = value;
            transform.localScale = Vector3.one * ToScale();
        }
    }

    public float ToScale() => ToScale(size);
    public static float ToScale(float x) => Mathf.Pow(x + 1, 0.5f) - 1;

    void Update()
    {
        var sizePow = Mathf.Pow(size + 1, 2f);
        var sizeTime = WorldConfig.ShrinkSpeed * sizePow * Time.deltaTime;
        var slow = (1 + cc.Rb.velocity.magnitude);
        var shrink = sizeTime / 700f;
        shrink += sizeTime / 1500f / slow * Mathf.Abs(cc.Rb.angularVelocity);
        shrink += sizeTime / 500f / slow;
        Size -= shrink;
    }
}
