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
    
    [SerializeField]
    private float speed;
    public float Speed
    {
        get => speed; 
        set
        {
            speed = value;
            Time.timeScale = value;
        }
    }

    [SerializeField]
    private Vector3 area;
    public Vector3 Area { get; set; }

    [SerializeField]
    private float cameraZoom;
    public float CameraZoom { get; set; }

    [SerializeField]
    private Vector2 cameraPos;
    public Vector2 CameraPos { get; set; }

    public void OnStart()
    {
        Time.timeScale = Speed;
    }
}