using UnityEngine;

[CreateAssetMenu(fileName = "ValhallaData", menuName = "ScriptableObjects/ValhallaData")]
public class ValhallaData : ScriptableObject
{
    public enum Metric
    {
        Speed,
        // Add other parameters here
    }

    //[field: SerializeField]
    // public int Speed;
    // Add other global simulation parameters here
}