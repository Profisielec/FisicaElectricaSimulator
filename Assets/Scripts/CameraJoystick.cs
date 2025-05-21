using UnityEngine;
using UnityEngine.EventSystems;

public class CameraJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform JoBackground;
    public RectTransform JoHandle;
    public Camera cameraToMove;
    public float moveSpeed = 5f;

    private Vector2 input;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(JoBackground, eventData.position, null, out pos);
        pos = Vector2.ClampMagnitude(pos, JoBackground.sizeDelta.x / 2);
        JoHandle.anchoredPosition = pos;
        input = pos / (JoBackground.sizeDelta.x / 2);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        input = Vector2.zero;
        JoHandle.anchoredPosition = Vector2.zero;
    }

    void Update()
    {
        if (cameraToMove != null)
        {
            Vector3 move = new Vector3(input.x, 0, input.y);
            cameraToMove.transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);
        }
    }
}