using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValuePair : MonoBehaviour
{

    [SerializeField] protected Slider slider;
    [SerializeField] TMP_Text text;
    [SerializeField] int digits;

    public float Value => Map(slider.value);

    void Awake() => slider.onValueChanged.AddListener(_ => UpdateText());
    void UpdateText() => text.text = Value.ToString($"F{digits}");
    virtual protected float Map(float value) => value;
}