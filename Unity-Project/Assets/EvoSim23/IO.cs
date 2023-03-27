using System.IO;
using UnityEngine;

public class IO : MonoBehaviour
{
    [SerializeField] WorldData worldData;
    [SerializeField] ValhallaData valhallaData;

    void OnEnable()
    {
        LoadWorld(worldData);
        //LoadValhalla(valhallaData);

        worldData.OnStart();
    }

    void OnDisable()
    {
        SaveWorld(worldData);
        //SaveValhalla(valhallaData);
    }

    void SaveWorld(WorldData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(GetPath(data.name), json);
    }

    void LoadWorld(WorldData data)
    {
        var path = GetPath(data.name);
        if (!File.Exists(path)) return;
        string json = File.ReadAllText(path);
        JsonUtility.FromJsonOverwrite(json, data);
    }

    public static void SaveHero(ValhallaData.Metric metric, HeroData data)
    {
        string dataJson = JsonUtility.ToJson(data);
        File.WriteAllText(GetPath("Valhalla " + metric.ToString()), dataJson);
    }

    void LoadValhalla(ValhallaData data)
    {

    }

    static string GetPath(string name) => Path.Combine(Application.persistentDataPath, name + ".json");
}