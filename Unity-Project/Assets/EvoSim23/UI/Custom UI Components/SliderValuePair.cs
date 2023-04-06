using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValuePair : MonoBehaviour
{
    [SerializeField] protected Slider slider;
    [SerializeField] TMP_Text text;
    public int UpdateDigits = 2;

    public float Value => Map(slider.value);

    void Awake() => slider.onValueChanged.AddListener(_ => UpdateText());
    protected void UpdateText() => text.text = Value.ToString($"F{UpdateDigits}");
    virtual protected float Map(float value) => value;
}