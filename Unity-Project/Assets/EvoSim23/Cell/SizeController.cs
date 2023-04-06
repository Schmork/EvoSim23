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
    public static float ToScale(float x) => math.sqrt(1 + x) - 1;

    [BurstCompile]
    void Update()
    {
        Size -= WorldConfig.ShrinkSpeed
                * size
                * Time.deltaTime
                * math.abs(cc.Rb.angularVelocity)
                / (0.3f + cc.Rb.velocity.magnitude);
    }
}
