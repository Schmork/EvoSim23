using Unity.Burst;
using UnityEngine;

[BurstCompile]
public class CollisionController : MonoBehaviour
{
    [SerializeField] CellController cc;
    float stomach;

    void OnEnable()
    {
        stomach = 0;        
    }

    [BurstCompile]
    void Update()
    {
        stomach -= cc.Size * 0.001f;
        if (stomach < 0)
        {
            cc.Stats.TimeHungry += Time.deltaTime;
            cc.Size += stomach * 0.06f * Time.deltaTime;
        }
        else
        {
            cc.Stats.TimeNotHungry += Time.deltaTime;
        }
    }

    [BurstCompile]
    void OnTriggerStay2D(Collider2D collider)
    {
        var other = collider.GetComponent<CellController>();
        if (other.Size > cc.Size) return;

        var morsel = Mathf.Sqrt(1 + cc.Size) * WorldConfig.AbsorbSpeed * Time.deltaTime;
        morsel = Mathf.Min(morsel, other.Size);

        Color.RGBToHSV(cc.Renderer.color, out float hue1, out _, out _);
        Color.RGBToHSV(other.Renderer.color, out float hue2, out _, out _);

        float hueDelta = hue2 * morsel / cc.Size;
        if (Mathf.Abs(hue1 - hue2) > 0.5f)
        {
            if (hue1 > hue2)
            {
                hue2 += 1f;
            }
            else
            {
                hue2 -= 1f;
            }
        }
        float hue = Mathf.Lerp(hue1, hue2, hueDelta);
        hue = (hue % 1 + 1) % 1;

        cc.Renderer.color = Color.HSVToRGB(hue, WorldConfig.FixedSatVal, WorldConfig.FixedSatVal);

        cc.Size += morsel * WorldConfig.AbsorbEfficiency;
        other.Size -= morsel;

        var score = morsel / cc.Size;

        cc.Stats.MassEaten += score;
        cc.Stats.MassAtSpeed += score * cc.Rb.velocity.magnitude;
        stomach += morsel;
    }
}