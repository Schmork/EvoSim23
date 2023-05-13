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
        MaxCells,
    }

    [SerializeField] private bool toggleValhalla;
    public bool ToggleValhalla { get => toggleValhalla; set => toggleValhalla = value; }

    [SerializeField] private bool toggleWorld;
    public bool ToggleWorld { get => toggleWorld; set => toggleWorld = value; }

    [SerializeField] private float fence;
    public float Fence { get => fence; set => fence = value; }

    [SerializeField] private float maxCells;
    public float MaxCells { get => maxCells; set => maxCells = value; }

    [SerializeField] private float cameraZoom;
    public float CameraZoom { get => cameraZoom; set => cameraZoom = value; }

    [SerializeField] private Vector2 cameraPos;
    public Vector2 CameraPos { get => cameraPos; set => cameraPos = value; }

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
    public float Gauss
    {
        get => gauss;
        set
        {
            gauss = value;
            GaussStd = value;
        }
    }
    public static float GaussStd;

    public void OnStart()
    {
        Time.timeScale = Speed;
        GaussStd = gauss;
    }
}