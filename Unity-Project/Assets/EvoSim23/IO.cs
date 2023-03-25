using System.IO;
using UnityEngine;

public class IO : MonoBehaviour
{
    [SerializeField] WorldData worldData;

    private void OnEnable()
    {
        var path = GetPath(worldData.name);
        if (!File.Exists(path)) return;
        string json = File.ReadAllText(path);
        JsonUtility.FromJsonOverwrite(json, worldData);
        worldData.OnStart();
    }

    private void OnDisable()
    {
        string json = JsonUtility.ToJson(worldData);
        File.WriteAllText(GetPath(worldData.name), json);
    }

    string GetPath(string name) => Path.Combine(Application.persistentDataPath, name + ".json");
}