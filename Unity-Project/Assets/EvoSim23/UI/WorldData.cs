using UnityEngine;

[CreateAssetMenu(fileName = "WorldData", menuName = "ScriptableObjects/WorldData")]
public class WorldData : ScriptableObject
{
    public enum Parameter
    {
        Speed,
        Gauss,
        Fence,
        Area,
        // Add other parameters here
    }
    
    [field: SerializeField]
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

    [field: SerializeField]
    private Vector3 _area;
    public Vector3 Area
    {
        get => _area;
        set => _area = value;
    }

    void OnEnable()
    {
        Time.timeScale = Speed;
    }
}