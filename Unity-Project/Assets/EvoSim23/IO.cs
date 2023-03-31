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
            property?.SetValue(worldData, Convert.ChangeType(kvp.Value, property.PropertyType));
        }
    }

    void LoadValhalla(ValhallaData valhalla)
    {
        foreach (Metric metric in Enum.GetValues(typeof(Metric)))
        {
            var path = GetPath("Valhalla " + metric.ToString());
            if (!File.Exists(path)) continue;
            var json = File.ReadAllText(path);
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

    public static void SaveHero(Metric metric, HeroData data)
    {
        var dataJson = JsonUtility.ToJson(data);
        File.WriteAllText(GetPath("Valhalla " + metric.ToString()), dataJson);
    }

    static string GetPath(string name) => Path.Combine(Application.persistentDataPath, name + ".json");
}