using UnityEngine;
using UnityEngine.UI;

public class TogglePanel : MonoBehaviour
{
    [SerializeField] Toggle toggle;
    [SerializeField] GameObject panel;
    [SerializeField] WorldData worldData;
    [SerializeField] WorldData.Parameter parameter;

    void Awake()
    {
        toggle.onValueChanged.AddListener(value =>
        {
            panel.SetActive(value);
            worldData.GetType()
                     .GetProperty(parameter.ToString())
                     .SetValue(worldData, value);
        });

        toggle.isOn = (bool)worldData.GetType()
                                     .GetProperty(parameter.ToString())
                                     .GetValue(worldData);
    }
}
