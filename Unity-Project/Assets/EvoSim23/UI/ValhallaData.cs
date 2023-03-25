using UnityEngine;

[CreateAssetMenu(fileName = "ValhallaData", menuName = "ScriptableObjects/ValhallaData", order = 1)]
public class ValhallaData : ScriptableObject
{
    public enum Metric
    {
        Speed,
        // Add other parameters here
    }

    // public int Speed;
    // Add other global simulation parameters here
}