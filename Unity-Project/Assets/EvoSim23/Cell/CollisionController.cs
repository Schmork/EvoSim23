using UnityEngine;

public class CollisionController : MonoBehaviour
{
    [SerializeField] CellController cc;

    void OnTriggerStay2D(Collider2D collider)
    {
        var other = collider.GetComponent<CellController>();
        if (other.Size > cc.Size) return;
        var diff = cc.Size * WorldConfig.AbsorbSpeed * Time.deltaTime;
        if (other.Size < diff) diff = other.Size;

        var ratio = diff / cc.Size;
        var col1 = cc.Renderer.color;
        var col2 = other.Renderer.color;
        cc.Renderer.color = MixColors(col1, col2, ratio);

        cc.Size += diff;
        other.Size -= diff;
    }
    private Color MixColors(Color color1, Color color2, float ratio)
    {
        // Convert colors to HSV
        Color.RGBToHSV(color1, out float h1, out _, out _);
        Color.RGBToHSV(color2, out float h2, out _, out _);

        // Find the weighted average of the hues
        float hue = Mathf.Lerp(h1, h2, ratio);

        // Create a new color with the mixed hue and the fixed saturation and value
        return Color.HSVToRGB(hue, 0.9f, 0.8f);
    }
}
