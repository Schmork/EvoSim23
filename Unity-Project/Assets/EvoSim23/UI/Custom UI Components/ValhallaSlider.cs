using TMPro;
using UnityEngine;

public class ValhallaSlider : SliderValuePair
{
    [SerializeField] ValhallaData valhallaData;
    [SerializeField] ValhallaData.Metric metric;
    [SerializeField] TMP_Text label, record;
    [SerializeField] int digits;

    void Start()
    {
        valhallaData.ScoreChanged += OnHeroAdded;
        slider.value = (float)typeof(ValhallaData).GetProperty(metric.ToString())
                                                  .GetValue(valhallaData);

        slider.onValueChanged.AddListener(value => typeof(ValhallaData).GetProperty(metric.ToString())
                                                                       .SetValue(valhallaData, value));

        label.text = metric.ToString();
        InvokeRepeating(nameof(Decay), 1, 1f);
    }

    void Decay() => valhallaData.DecayScores(metric);

    void OnHeroAdded(ValhallaData.Metric metric, float score)
    {
        if (metric != this.metric) return;
        record.text = score.ToString($"F{digits}");
    }
}