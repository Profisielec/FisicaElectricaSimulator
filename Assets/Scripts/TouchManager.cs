using UnityEngine;

public class TouchManager : MonoBehaviour
{
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);

                if (hit.collider != null)
                {
                    ForceLine linea = hit.collider.GetComponent<ForceLine>();
                    if (linea != null)
                    {
                        UIManager.instance.MostrarFuerza(linea.fuerzaActual);
                    }
                }
            }
        }
    }
}
