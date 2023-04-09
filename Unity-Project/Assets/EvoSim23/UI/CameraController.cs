using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [SerializeField] WorldData worldData;

    [SerializeField] float panSpeed = 0.1f;
    [SerializeField] float zoomSpeed = 0.7f;
    [SerializeField] float minZoom = 5;
    [SerializeField] float maxZoom = 1000;

    Vector3 panStart;
    Vector3 lastPanPosition;
    float zoomStart;

    void Start()
    {
        zoomStart = worldData.CameraZoom;
        transform.position = worldData.CameraPos;
    }

    void Update()
    {
        //if (!Input.GetMouseButton(0) || 
        //    !Input.GetMouseButtonDown(0) || 
        //    !Input.GetMouseButtonDown(2)) return;

        float adjustedPanSpeed = panSpeed * Mathf.Pow(zoomStart, 0.3f);

        if (Input.GetMouseButtonDown(0) && EventSystem.current.currentSelectedGameObject == null)
        {
            panStart = GetWorldPositionFromScreen(Input.mousePosition);
            lastPanPosition = panStart;
        }
        if (Input.GetMouseButton(0) && EventSystem.current.currentSelectedGameObject == null)
        {
            Vector3 currentPanPosition = GetWorldPositionFromScreen(Input.mousePosition);
            Vector3 panOffset = lastPanPosition - currentPanPosition;
            transform.position += panOffset * adjustedPanSpeed;
            lastPanPosition = currentPanPosition;
        }

        float zoomDelta = -Input.mouseScrollDelta.y * zoomSpeed;
        zoomStart = Mathf.Clamp(zoomStart + zoomDelta, minZoom, maxZoom);
        Camera.main.orthographicSize = zoomStart;

        if (Input.GetMouseButtonDown(2))
        {
            transform.position = Vector3.zero;
            zoomStart = (minZoom + maxZoom) / 2;
            Camera.main.orthographicSize = zoomStart;
        }

        worldData.CameraPos = transform.position;
        worldData.CameraZoom = zoomStart;
    }

    Vector3 GetWorldPositionFromScreen(Vector3 screenPosition) => Camera.main.ScreenToWorldPoint(screenPosition);
}