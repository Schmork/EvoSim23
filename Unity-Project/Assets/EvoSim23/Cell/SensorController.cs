using UnityEngine;
using Unity.Burst;
using Unity.Mathematics;

public class SensorController : MonoBehaviour
{
    [SerializeField] CellController cc;
    [SerializeField] LayerMask layer;

    const float sensorRadius = 15f;
    const float comparisonSafety = 0.98f; // reduce own size in comparisons (safety margin)
    readonly public static int numSensorValues = 4;

    struct TrackedCell
    {
        internal Collider2D col;
        internal float value;

        internal TrackedCell(float val)
        {
            col = null;
            value = val;
        }
    }

    [BurstCompile]
    public float4 Scan()
    {
        var hits = Physics2D.OverlapCircleAll(transform.position, 
                                              SizeController.ToScale(cc.Size) * sensorRadius, 
                                              layer);

        TrackedCell prey = new();
        TrackedCell threat = new(float.MaxValue);

        var mySize = transform.localScale.magnitude * comparisonSafety;

        for (int i = 0; i < hits.Length; i++)
        {
            var hit = hits[i];
            if (hit.gameObject == gameObject) continue;

            var otherSize = hit.transform.localScale.magnitude;
            var distance = Vector2.Distance(transform.position, FuturePosition(hit));

            if (mySize > otherSize)
            {
                prey = CheckSmaller(prey, hit, distance, otherSize);
            }
            else
            {
                threat = CheckBigger(threat, hit, distance);
            }
        }

        float4 output;
        output.w = prey.col == null ? 0 : 1;
        output.x = Direction(prey.col) ?? 0;
        output.y = threat.col == null ? 0 : -1;
        output.z = Direction(threat.col) ?? 0;
        return output;
    }

    TrackedCell CheckBigger(TrackedCell threat, Collider2D col, float distance)
    {
        if (distance < threat.value)
        {
            threat.col = col;
            threat.value = distance;
        }
        return threat;
    }

    TrackedCell CheckSmaller(TrackedCell prey, Collider2D col, float distance, float size)
    {
        var value = size * size / (distance * distance);
        if (value > prey.value)
        {
            prey.col = col;
            prey.value = value;
        }
        return prey;
    }

    float? Direction(Collider2D other)
    {
        if (other == null) return null;
        return Vector2.SignedAngle(transform.up, FuturePosition(other) - (Vector2)transform.position) / 180f;
    }

    Vector2 FuturePosition(Collider2D other)
    {
        return (Vector2)other.transform.position
               + other.transform.up
               * other.attachedRigidbody.velocity
               * Time.deltaTime;
    }
}