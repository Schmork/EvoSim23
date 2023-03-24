using UnityEngine;
using UnityEngine.UI;

public class TogglePanel : MonoBehaviour
{
    [SerializeField] Toggle toggle;
    [SerializeField] GameObject panel;

    void Awake()
    {
        toggle.onValueChanged.AddListener(value => panel.SetActive(value));
        panel.SetActive(toggle.isOn);
    }
}
