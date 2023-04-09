using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValuePair : MonoBehaviour
{
    [SerializeField] protected Slider slider;
    [SerializeField] protected TMP_Text text;
    [SerializeField] protected int ChanceDigits;
    [SerializeField] protected int RecordDigits;

    public float Value => Map(slider.value);

    void Awake() => slider.onValueChanged.AddListener(_ => UpdateText());
    protected void UpdateText() => text.text = Value.ToString($"F{ChanceDigits}");
    virtual protected float Map(float value) => value;
}