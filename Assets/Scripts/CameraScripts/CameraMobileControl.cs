using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMobileControl : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float lookSpeed = 2f;
    public Joystick joystick; // Asigna el Joystick en el Inspector o lo buscará automáticamente

    private float rotationX = 0f;
    private float rotationY = 0f;
    private Vector2 lastTouchPosition;
    private bool isDragging = false;

    void Start()
    {
        // Si el joystick no está asignado en el Inspector, intenta encontrarlo en la escena
        if (joystick == null)
        {
            joystick = GameObject.Find("Floating Joystick")?.GetComponent<Joystick>();

            if (joystick == null)
            {
                Debug.LogError("Joystick no encontrado. Asegúrate de que el nombre es correcto y que está en la escena.");
            }
        }
    }

    void Update()
    {
        MoveWithJoystick();
        RotateWithTouch();
    }

    void MoveWithJoystick()
    {
        if (joystick == null) return;

        float moveX = joystick.Horizontal * moveSpeed * Time.deltaTime;
        float moveZ = joystick.Vertical * moveSpeed * Time.deltaTime;

        transform.position += transform.right * moveX + transform.forward * moveZ;
    }

    void RotateWithTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                lastTouchPosition = touch.position;
                isDragging = true;
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                Vector2 delta = touch.deltaPosition;
                rotationY += delta.x * lookSpeed * Time.deltaTime;
                rotationX -= delta.y * lookSpeed * Time.deltaTime;
                rotationX = Mathf.Clamp(rotationX, -90f, 90f);
                transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                isDragging = false;
            }
        }
    }
}