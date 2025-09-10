using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public RectTransform baseRT;
    public RectTransform knobRT;
    public float maxRadius = 100f;
    public float deadZone = 0.15f;
    public float sensitivity = 1.0f;
    Vector2 input;

    public Vector2 Value => input;

    public void OnPointerDown(PointerEventData e) { OnDrag(e); }
    public void OnPointerUp(PointerEventData e) { input = Vector2.zero; knobRT.anchoredPosition = Vector2.zero; }
    public void OnDrag(PointerEventData e)
    {
        Vector2 local;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(baseRT, e.position, e.pressEventCamera, out local);
        var v = Vector2.ClampMagnitude(local, maxRadius) / maxRadius;
        float m = v.magnitude;
        if (m < deadZone) v = Vector2.zero;
        else v = v.normalized * Mathf.InverseLerp(deadZone, 1f, m);
        v *= sensitivity;
        knobRT.anchoredPosition = v * maxRadius;
        input = Vector2.ClampMagnitude(v, 1f);
    }
}
