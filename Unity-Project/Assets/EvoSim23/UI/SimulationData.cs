using UnityEngine;

[CreateAssetMenu(fileName = "SimulationData", menuName = "ScriptableObjects/SimulationData", order = 1)]
public class SimulationData : ScriptableObject
{
    public enum Parameter
    {
        Speed,
        // Add other parameters here
    }

    private float _speed;
    public float Speed
    {
        get => _speed; 
        set
        {
            _speed = value;
            Time.timeScale = value;
        }
    }

    void OnEnable()
    {
        Time.timeScale = Speed;
    }
}