using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        valhallaData.OnStart();
        SceneManager.LoadScene("World", LoadSceneMode.Additive);
    }

    void OnDisable()
    {
        SaveWorld(worldData);
        SaveValhalla(valhallaData);
    }

    void SaveWorld(WorldData data)
    {
        var path = GetPath("WorldData");
        var dataJson = JsonUtility.ToJson(data);
        File.WriteAllText(path, dataJson);
    }

    void LoadWorld(WorldData data)
    {
        var path = GetPath("WorldData");
        var json = File.ReadAllText(path);
        var jsonObject = JsonUtility.FromJson<Dictionary<string, object>>(json);

        foreach (var kvp in jsonObject)
        {
            var property = typeof(WorldData).GetProperty(kvp.Key);
            property?.SetValue(data, Convert.ChangeType(kvp.Value, property.PropertyType));
        }
    }

    void LoadValhalla(ValhallaData data)
    {
        string path, json;

        foreach (Metric metric in Enum.GetValues(typeof(Metric)))
        {
            path = GetPath("Valhalla " + metric.ToString());
            if (!File.Exists(path)) continue;
            json = File.ReadAllText(path);
            var hero = JsonUtility.FromJson<HeroData>(json);
            data.Heroes[(int)metric] = hero;
        }

        path = GetPath("ValhallaData");
        if (!File.Exists(path)) return;
        json = File.ReadAllText(path);
        var jsonObject = JsonUtility.FromJson<Dictionary<string, object>>(json);

        foreach (var kvp in jsonObject)
        {
            var property = typeof(ValhallaData).GetProperty(kvp.Key);
            property?.SetValue(data, Convert.ChangeType(kvp.Value, property.PropertyType));
        }
    }

    private void SaveValhalla(ValhallaData data)
    {
        var path = GetPath("ValhallaData");
        var dataJson = JsonUtility.ToJson(data);
        File.WriteAllText(path, dataJson);

        for (int i = 0; i < data.Heroes.Length; i++)
        {
            if ((Metric)i == Metric.NewRandom) continue;
            SaveHero((Metric)i, data.Heroes[i]);
        }
    }

    public static void SaveHero(Metric metric, HeroData data)
    {
        var dataJson = JsonUtility.ToJson(data);
        File.WriteAllText(GetPath("Valhalla " + metric.ToString()), dataJson);
    }

    public static void WipeAll()
    {
        foreach (Metric metric in Enum.GetValues(typeof(Metric)))
        {
            var path = GetPath("Valhalla " + metric.ToString());
            if (File.Exists(path)) File.Delete(path);
        }
    }

    static string GetPath(string name) => Path.Combine(Application.persistentDataPath, name + ".json");
}