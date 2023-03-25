using UnityEngine;

public class WorldSlider : SliderValuePair
{
    public enum ValueMapping
    {
        Identity,
        Exponential,
        SteppedIncrease,
    }

    [SerializeField] ValueMapping mapping;
    [SerializeField] WorldData worldData;
    [SerializeField] WorldData.Parameter parameter;

    void Start()
    {
        slider.value = (float)worldData.GetType()
                                       .GetProperty(parameter.ToString())
                                       .GetValue(worldData);

        slider.onValueChanged.AddListener(value => worldData.GetType()
                                                            .GetProperty(parameter.ToString())
                                                            .SetValue(worldData, value));
    }

    protected override float Map(float value)
    {
        return mapping switch
        {
            ValueMapping.Identity => value,
            ValueMapping.Exponential => Mathf.Exp(value),
            ValueMapping.SteppedIncrease => SteppedIncrease(value),
            _ => throw new System.ArgumentException($"Unknown mapping type {mapping}")
        };
    }

    float SteppedIncrease(float value)
    {
        if (value < 10)
        {
            value /= 10f;
        }
        else value -= 9;
        if (value > 15)
            value = (int)Mathf.Pow(value, 1.2f) - 10;
        return value;
    }
}
