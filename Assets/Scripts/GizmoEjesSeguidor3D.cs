using UnityEngine;

public class GizmoEjesSeguidor3D : MonoBehaviour
{
    public Transform camara;
    public Vector3 offsetPantalla = new Vector3(-2f, 1.5f, 5f);  // posici�n frente a la c�mara

    void LateUpdate()
    {
        if (camara == null) return;

        // Pone el gizmo frente a la c�mara con un offset local
        transform.position = camara.position + camara.TransformDirection(offsetPantalla);

        // Rota igual que la c�mara
        transform.rotation = camara.rotation;
    }
}
