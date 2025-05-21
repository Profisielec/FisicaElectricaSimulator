using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camerascrip : MonoBehaviour
{
    public float moveSpeed= 10f;
    public float sprintMultiplier=2f;
    public float lookSpeed=2f;
    
    float rotationX = 0f;
    float rotationY = 0;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    // Update is called once per frame
    void Update()
    {
        //Movimiento con el teclado
        float speed = moveSpeed * (Input.GetKey(KeyCode.LeftShift) ? sprintMultiplier : 1f);//Acelerador de movimiento
        float moveX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float moveY = 0;//Ya que solo cambia cuando se presionan las teclas Q o E

        if (Input.GetKey(KeyCode.Q)) moveY = -speed * Time.deltaTime;//Baja
        if (Input.GetKey(KeyCode.E)) moveY = speed * Time.deltaTime; //Sube

        transform.position += transform.right * moveX + transform.forward * moveZ + transform.up * moveY;

        //Movimiento del mouse en X y en Y
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeed;
        rotationY += Input.GetAxis("Mouse X") * lookSpeed;

        rotationX = Mathf.Clamp(rotationX, -90f, 90f);//Evita movimientos anormales de la camara

        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
    }
}
