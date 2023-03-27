using System;
using UnityEngine;


[Serializable]
public class HeroData
{
    public float Score;
    public NeuralNetwork Network;
}

[CreateAssetMenu(fileName = "ValhallaData", menuName = "ScriptableObjects/ValhallaData")]
[Serializable]
public class ValhallaData : ScriptableObject
{
    public enum Metric
    {
        DistanceTravelled,
        TimeSurvived,
        MassEaten
    }

    public HeroData[] Heroes { get; private set; }

    void OnEnable()
    {
        Heroes = new HeroData[Enum.GetNames(typeof(Metric)).Length];
        for (int i = 0; i < Heroes.Length; i++)
        {
            Heroes[i] = new HeroData();
        }
    }

    public void AddHero(Metric metric, float score, NeuralNetwork network)
    {
        var i = (int)metric;

        if (score < Heroes[i].Score) return;

        Heroes[i].Score = score;
        Heroes[i].Network = network;

        IO.SaveHero(metric, Heroes[i]);
    }

    public NeuralNetwork GetHero()
    {
        var i = (int)(UnityEngine.Random.value * Heroes.Length);
        if (Heroes[i].Network == null)
            return NeuralNetwork.NewRandom();
        else
            return Heroes[i].Network.Clone() as NeuralNetwork;
    }
}