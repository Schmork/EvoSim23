using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValuePair : MonoBehaviour
{
    public enum ValueMapping
    {
        Identity,
        Exponential,
        SteppedIncrease,
    }

    [SerializeField] Slider slider;
    [SerializeField] TMP_Text text;
    [SerializeField] int digits;
    [SerializeField] ValueMapping mapping;

    public float Value => slider.value;

    void Awake()
    {
        slider.onValueChanged.AddListener(_ => UpdateText());
        UpdateText();
    }

    void UpdateText() => text.text = Map(Value).ToString($"F{digits}");

    float Map(float value)
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
