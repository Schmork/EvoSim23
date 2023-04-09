using TMPro;
using UnityEngine;

public class DecaySlider : SliderValuePair
{
    [SerializeField] ValhallaData valhallaData;
    [SerializeField] string property = "DecaySpeed";

    void Start()
    {

        slider.onValueChanged.AddListener(value =>
        {
            typeof(ValhallaData).GetProperty(property)
                                .SetValue(valhallaData, value);
            
            text.text = value.ToString($"F{RecordDigits}");
        }); 
        
        slider.value = (float)typeof(ValhallaData).GetProperty(property)
                                                  .GetValue(valhallaData);
    }
}