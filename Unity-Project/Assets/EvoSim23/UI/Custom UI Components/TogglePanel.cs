using UnityEngine;
using UnityEngine.UI;

public class TogglePanel : MonoBehaviour
{
    enum ToggleType
    {
        World,
        Valhalla,
    }

    [SerializeField] private ToggleType type;
    [SerializeField] Toggle toggle;
    [SerializeField] GameObject panel;
    [SerializeField] WorldData worldData;
    [SerializeField] WorldData.Parameter parameter;

    void Start()
    {
        toggle.onValueChanged.AddListener(value =>
        {
            panel.SetActive(value);

            switch (type)
            {
                case ToggleType.World:
                    worldData.ToggleWorld = value;
                    break;
                case ToggleType.Valhalla:
                    worldData.ToggleValhalla = value;
                    break;
            }
        });

        toggle.isOn = type switch
        {
            ToggleType.World => worldData.ToggleWorld,
            ToggleType.Valhalla => worldData.ToggleValhalla,
            _ => throw new System.NotImplementedException(),
        };
    }
}
