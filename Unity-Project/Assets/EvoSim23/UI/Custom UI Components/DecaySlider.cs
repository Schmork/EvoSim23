using UnityEngine;

public class DecaySlider : SliderValuePair
{
    [SerializeField] ValhallaData valhallaData;
    [SerializeField] string property = "DecaySpeed";

    void Start()
    {
        slider.value = (float)typeof(ValhallaData).GetProperty(property)
                                                  .GetValue(valhallaData);

        slider.onValueChanged.AddListener(value => typeof(ValhallaData).GetProperty(property)
                                                                       .SetValue(valhallaData, value));
        UpdateText();
    }
}