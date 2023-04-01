using UnityEngine;

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
            cc.Valhalla.AddHero(ValhallaData.Metric.MassEaten, value, cc.NeuralNetwork);
        }
    }

    void Update()
    {
        TimeSurvived += Time.deltaTime;
        DistanceTravelled += cc.Rb.velocity.magnitude * Time.deltaTime;
    }

    void OnDisable()
    {
        distanceTravelled = 0;
        massEaten = 0;
        timeSurvived = 0;
    }
}
