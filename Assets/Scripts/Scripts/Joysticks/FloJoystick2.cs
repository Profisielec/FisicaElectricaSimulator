using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloJoystick2 : Joystick
{
    protected override void Start()
    {
        base.Start();

        if (background != null)
        {
            background.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("FloatingJoystick: ¡No se ha asignado el 'background' en el Inspector!");
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (background != null)
        {
            background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            background.gameObject.SetActive(true);
        }
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (background != null)
        {
            background.gameObject.SetActive(false);
        }
        base.OnPointerUp(eventData);
    }
}
