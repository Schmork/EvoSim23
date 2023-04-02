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

        var hue = Mathf.LerpAngle(hue1, hue2, morsel / cc.Size);
        cc.Renderer.color = Color.HSVToRGB(hue, WorldConfig.FixedSatVal, WorldConfig.FixedSatVal);

        cc.Size += morsel * WorldConfig.AbsorbEfficiency;
        other.Size -= morsel;

        cc.Stats.MassEaten += morsel;
    }
}