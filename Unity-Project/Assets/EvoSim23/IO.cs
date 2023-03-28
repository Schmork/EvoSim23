using System;
using System.IO;
using UnityEngine;
using static ValhallaData;

public class IO : MonoBehaviour
{
    [SerializeField] WorldData worldData;
    [SerializeField] ValhallaData valhallaData;

    void OnEnable()
    {
        LoadWorld(worldData);
        LoadValhalla(valhallaData);

        worldData.OnStart();
    }

    void OnDisable()
    {
        SaveWorld(worldData);
        SaveValhalla(valhallaData);
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

    public static void SaveHero(Metric metric, HeroData data)
    {
        string dataJson = JsonUtility.ToJson(data);
        File.WriteAllText(GetPath("Valhalla " + metric.ToString()), dataJson);
    }

    void LoadValhalla(ValhallaData valhalla)
    {
        foreach (Metric metric in Enum.GetValues(typeof(Metric)))
        {
            var path = GetPath("Valhalla " + metric.ToString());
            if (!File.Exists(path)) continue;
            string json = File.ReadAllText(path);
            var hero = JsonUtility.FromJson<HeroData>(json);
            valhalla.Heroes[(int)metric] = hero;
        }
    }

    private void SaveValhalla(ValhallaData valhallaData)
    {
        for (int i = 0; i < valhallaData.Heroes.Length; i++)
        {
            SaveHero((Metric)i, valhallaData.Heroes[i]);
        }
    }

    static string GetPath(string name) => Path.Combine(Application.persistentDataPath, name + ".json");
}