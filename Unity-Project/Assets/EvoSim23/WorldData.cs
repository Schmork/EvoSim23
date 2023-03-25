using UnityEngine;

[CreateAssetMenu(fileName = "WorldData", menuName = "ScriptableObjects/WorldData")]
public class WorldData : ScriptableObject
{
    public enum Parameter
    {
        ToggleValhalla,
        ToggleWorld,
        Speed,
        Gauss,
        Fence,
        Area,
    }

    [SerializeField] bool toggleValhalla;
    public bool ToggleValhalla { get => toggleValhalla; set => toggleValhalla = value; }

    [SerializeField] bool toggleWorld;
    public bool ToggleWorld { get => toggleWorld; set => toggleWorld = value; }

    [SerializeField] float speed;
    public float Speed
    {
        get => speed;
        set
        {
            speed = value;
            Time.timeScale = value;
        }
    }

    [SerializeField] float gauss;
    public float Gauss { get => gauss; set => gauss = value; }

    [SerializeField] float fence;
    public float Fence { get => fence; set => fence = value; }

    [SerializeField] Vector3 area;
    public Vector3 Area { get => area; set => area = value; }

    [SerializeField] float cameraZoom;
    public float CameraZoom { get => cameraZoom; set => cameraZoom = value; }

    [SerializeField] Vector2 cameraPos;
    public Vector2 CameraPos { get => cameraPos; set => cameraPos = value; }

    public void OnStart()
    {
        Time.timeScale = Speed;
    }
}