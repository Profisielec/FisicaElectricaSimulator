using UnityEngine;
using UnityEngine.UI;

public class PunteroColocador : MonoBehaviour
{
    public RectTransform punteroUI;        // Icono 2D visible
    public Transform puntero3D;            // Posición donde se coloca la partícula
    public Camera mainCam;
    public LayerMask planoLayer;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            // Mover puntero UI a posición de dedo
            punteroUI.position = touch.position;

            // Lanzar rayo desde pantalla al mundo 3D
            Ray ray = mainCam.ScreenPointToRay(touch.position);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, planoLayer))
            {
                puntero3D.position = hit.point;
            }
        }
    }
}
