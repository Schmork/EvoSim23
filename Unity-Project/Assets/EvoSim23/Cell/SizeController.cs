using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

[BurstCompile]
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

    [BurstCompile]
    void Update()
    {
        var sizeTime = WorldConfig.ShrinkSpeed * math.pow(size + 1, 3f) * Time.deltaTime;
        Size -= sizeTime / math.sqrt(1 + cc.Rb.velocity.magnitude) * math.sqrt(1 + Mathf.Abs(cc.Rb.angularVelocity));
    }
}
