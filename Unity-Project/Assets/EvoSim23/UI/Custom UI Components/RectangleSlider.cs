using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RectangleSlider : MonoBehaviour, IPointerDownHandler, IDragHandler, IEventSystemHandler
{
    [SerializeField] WorldData worldData;
    [SerializeField] Slider scaleSlider;
    [SerializeField] RectTransform knob;
    [SerializeField] RectTransform slideArea;
    [SerializeField] RectTransform valueArea;
    [SerializeField] TMP_Text text;

    [SerializeField]
    Vector3 _value;
    public Vector3 Value
    {
        get => _value;
        set
        {
            _value = value;

            text.text = (value.x * value.z).ToString("F1") + " • " +
                        (value.y * value.z).ToString("F1");

            Vector2 restoredPosition = new(value.x, value.y);
            knob.anchoredPosition = restoredPosition;
            valueArea.sizeDelta = restoredPosition;
            scaleSlider.value = value.z;

            worldData.Area = value;
        }
    }

    void Awake()
    {
        Value = worldData.Area;
        scaleSlider.onValueChanged.AddListener(value => Value = new Vector3(_value.x, _value.y, value));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
        UpdateArea(eventData);
    }

    public void OnDrag(PointerEventData eventData) => UpdateArea(eventData);

    void UpdateArea(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(slideArea,
                                                                    eventData.position,
                                                                    eventData.pressEventCamera,
                                                                    out Vector2 localPoint))
        {
            Vector2 clampedLocalPoint = new(
                Mathf.Clamp(localPoint.x, 0, slideArea.rect.width),
                Mathf.Clamp(localPoint.y, 0, slideArea.rect.height)
            );
            knob.anchoredPosition = clampedLocalPoint;
            valueArea.sizeDelta = clampedLocalPoint;
            Value = new Vector3(clampedLocalPoint.x, clampedLocalPoint.y, _value.z);
        }
    }
}