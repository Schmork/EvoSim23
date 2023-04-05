using Unity.Burst;
using UnityEngine;

[BurstCompile]
public class CollisionController : MonoBehaviour
{
    [SerializeField] CellController cc;

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

        cc.Stats.MassEaten += morsel;
    }
}