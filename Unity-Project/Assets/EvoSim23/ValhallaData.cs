using System;
using System.Linq;
using UnityEngine;

[Serializable]
public struct HeroData
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
        MassEaten,
        MassPerTime,
        MassAtSpeed,
        Diversity,
        TimeNotHungry,
    }

    [SerializeField] float _distanceTravelled;
    public float DistanceTravelled
    {
        get => _distanceTravelled; set
        {
            _distanceTravelled = value;
            UpdateSum();
        }
    }

    [SerializeField] float _timeSurvived;
    public float TimeSurvived
    {
        get => _timeSurvived; set
        {
            _timeSurvived = value;
            UpdateSum();
        }
    }

    [SerializeField] float _massEaten;
    public float MassEaten
    {
        get => _massEaten; set
        {
            _massEaten = value;
            UpdateSum();
        }
    }

    [SerializeField] float _decaySpeed;
    public float DecaySpeed
    {
        get => _decaySpeed; set
        {
            _decaySpeed = value;
            UpdateSum();
        }
    }

    [SerializeField] float _massPerTime;
    public float MassPerTime
    {
        get => _massPerTime; set
        {
            _massPerTime = value;
            UpdateSum();
        }
    }

    [SerializeField] float _massAtSpeed;
    public float MassAtSpeed
    {
        get => _massAtSpeed; set
        {
            _massAtSpeed = value;
            UpdateSum();
        }
    }

    [SerializeField] float _diversity;
    public float Diversity
    {
        get => _diversity; set
        {
            _diversity = value;
            UpdateSum();
        }
    }

    [SerializeField] float _timeNotHungry;
    public float TimeNotHungry
    {
        get => _timeNotHungry; set
        {
            _timeNotHungry = value;
            UpdateSum();
        }
    }

    public HeroData[] Heroes { get; private set; }

    public event Action<Metric, float> ScoreChanged;

    float[] chances;
    float sum = 0;

    void UpdateSum() => sum = chances.Sum();

    void OnEnable()
    {
        chances = new float[Enum.GetNames(typeof(Metric)).Length];
        Init();
    }

    void Init()
    {
        Heroes = new HeroData[Enum.GetNames(typeof(Metric)).Length];
        for (int i = 0; i < Heroes.Length; i++)
        {
            Heroes[i] = new HeroData();
            ScoreChanged?.Invoke((Metric)i, 0);
        }
    }

    public void OnStart()
    {
        foreach (Metric metric in Enum.GetValues(typeof(Metric)))
        {
            ScoreChanged?.Invoke(metric, Heroes[(int)metric].Score);

            var propertyName = metric.ToString();
            var property = GetType().GetProperty(propertyName);

            if (property != null && property.PropertyType == typeof(float))
            {
                chances[(int)metric] = (float)property.GetValue(this);
            }
        }
        UpdateSum();
    }

    public void DecayScores(Metric metric)
    {
        Heroes[(int)metric].Score *= (1 - _decaySpeed);
        ScoreChanged?.Invoke(metric, Heroes[(int)metric].Score);
    }

    public bool AddHero(Metric metric, float score, NeuralNetwork network)
    {
        var i = (int)metric;
        if (score < Heroes[i].Score) return false;

        Heroes[i].Score = score;
        Heroes[i].Network = network;
        ScoreChanged?.Invoke(metric, score);
        return true;
    }

    public NeuralNetwork GetHero()
    {
        var i = PickMetric();
        if (Heroes[i].Network == null)
            return NeuralNetwork.NewRandom();
        else
            return Heroes[i].Network.Clone() as NeuralNetwork;
    }

    public int PickMetric()
    {
        float randomValue = UnityEngine.Random.value * sum;
        var tempSum = 0f;

        for (int i = 0; i < chances.Length; i++)
        {
            tempSum += chances[i];
            if (randomValue < tempSum) return i;
        }

        Debug.Log("Error when picking metric");
        return (int)(UnityEngine.Random.value * Heroes.Length);
    }

    public void OnButtonWipePressed()
    {
        IO.WipeAll();
        Init();
        foreach (Metric metric in Enum.GetValues(typeof(Metric)))
        {
            ScoreChanged?.Invoke(metric, Heroes[(int)metric].Score);
        }

        foreach (var cell in FindObjectsByType<CellController>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            cell.Size = float.MinValue;
    }
}