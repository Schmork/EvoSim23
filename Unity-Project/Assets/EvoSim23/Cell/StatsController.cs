using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

[BurstCompile]
public class StatsController : MonoBehaviour
{
    [SerializeField] CellController cc;

    [SerializeField] float distanceTravelled;
    public float DistanceTravelled
    {
        get => distanceTravelled;
        set
        {
            distanceTravelled = value;
            cc.Valhalla.AddHero(ValhallaData.Metric.DistanceTravelled, value, cc.NeuralNetwork);
        }
    }

    [SerializeField] float timeSurvived;
    public float TimeSurvived
    {
        get => timeSurvived;
        set
        {
            timeSurvived = value;
            cc.Valhalla.AddHero(ValhallaData.Metric.TimeSurvived, value, cc.NeuralNetwork);
        }
    }

    [SerializeField] float massEaten;
    public float MassEaten
    {
        get => massEaten;
        set
        {
            massEaten = value;
            MassPerTime = MassEaten / math.sqrt(1 + TimeSurvived);
            if (cc.Valhalla.AddHero(ValhallaData.Metric.MassEaten, value, cc.NeuralNetwork))
                SaveSlowStats();
        }
    }

    [SerializeField] float massPerTime;
    public float MassPerTime
    {
        get => massPerTime;
        set
        {
            massPerTime = value;
        }
    }

    [SerializeField] float massAtSpeed;
    public float MassAtSpeed
    {
        get => massAtSpeed;
        set
        {
            massAtSpeed = value;
            if (cc.Valhalla.AddHero(ValhallaData.Metric.MassAtSpeed, value, cc.NeuralNetwork))
                SaveSlowStats();
        }
    }

    [SerializeField] float diversity;
    public float Diversity
    {
        get => diversity;
        set
        {
            diversity = value;
        }
    }

    [SerializeField] float timeHungry;
    public float TimeHungry
    {
        get => timeHungry;
        set
        {
            timeHungry = value;
        }
    }

    [SerializeField] float timeNotHungry;
    public float TimeNotHungry
    {
        get => timeNotHungry;
        set
        {
            timeNotHungry = value;
        }
    }

    [BurstCompile]
    void LateUpdate()
    {
        TimeSurvived += Time.deltaTime;
        DistanceTravelled += cc.Rb.velocity.magnitude * Time.deltaTime;
        Diversity += Vector2.SignedAngle(transform.up, cc.Rb.velocity) * Time.deltaTime;
    }

    void OnDisable()
    {
        SaveSlowStats();
        distanceTravelled = 0;
        timeSurvived = 0;
        massEaten = 0;
        massPerTime = 0;
        massAtSpeed = 0;
        diversity = 0;
        timeHungry = 0;
        timeNotHungry = 0;
    }

    void SaveSlowStats()
    {
        var rating = DistanceTravelled < 100 ? 0 : math.sqrt(1 + MassAtSpeed)
                     / (0.001f + Diversity * Diversity);
        cc.Valhalla.AddHero(ValhallaData.Metric.Diversity, rating, cc.NeuralNetwork);

        cc.Valhalla.AddHero(ValhallaData.Metric.MassPerTime, MassPerTime, cc.NeuralNetwork);

        var hungryScore = math.sqrt(1 + DistanceTravelled) * TimeNotHungry / TimeSurvived;
        cc.Valhalla.AddHero(ValhallaData.Metric.TimeNotHungry, hungryScore, cc.NeuralNetwork);
    }
}