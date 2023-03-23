using UnityEngine;

public class SensorController : MonoBehaviour
{
    [SerializeField] CellController cc;
    [SerializeField] LayerMask layer;

    const float sensorRadius = 10f;
    const float comparisonSafety = 0.9f; // reduce own size in comparisons (safety margin)

    public float updateInterval;
    float lastUpdate = 0f;
    readonly Collider2D[] hitsBuffer = new Collider2D[50];

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

    void Update()
    {
        if (Time.time - lastUpdate < updateInterval) return;

        var hits = Physics2D.OverlapCircleNonAlloc(transform.position, sensorRadius, hitsBuffer, layer);

        TrackedCell prey = new();
        TrackedCell threat = new(float.MaxValue);

        var mySize = transform.localScale.magnitude * comparisonSafety;

        for (int i = 0; i < hits; i++)
        {
            var hit = hitsBuffer[i];
            if (hit.gameObject == gameObject) continue;

            var otherSize = hit.transform.localScale.magnitude;
            var distance = Vector2.Distance(transform.position, hit.transform.position);

            if (mySize > otherSize)
            {
                prey = ProcessSmaller(prey, hit, distance, otherSize);
            }
            else
            {
                threat = ProcessBigger(threat, hit, distance);
            }
        }

        cc.sensorHasPrey = prey.col != null;
        cc.sensorHasThreat = threat.col != null;

        cc.sensorPreyDir = cc.sensorHasPrey ? Direction(prey.col) : 0;
        cc.sensorThreatDir = cc.sensorHasThreat ? Direction(threat.col) : 0;

        lastUpdate = Time.time;
    }

    TrackedCell ProcessBigger(TrackedCell threat, Collider2D col, float distance)
    {
        if (distance < threat.value)
        {
            threat.col = col;
            threat.value = distance;
        }
        return threat;
    }

    TrackedCell ProcessSmaller(TrackedCell prey, Collider2D col, float distance, float size)
    {
        var value = size / distance;
        if (value > prey.value)
        {
            prey.col = col;
            prey.value = value;
        }
        return prey;
    }

    float Direction(Collider2D other)
    {
        return Vector2.SignedAngle(transform.up, other.transform.position - transform.position) / 180f;
    }
}