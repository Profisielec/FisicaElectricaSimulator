using UnityEngine;

public class GizmoEjesSeguidor3D : MonoBehaviour
{
    public Transform camara;
    public Vector3 offsetPantalla = new Vector3(-2f, 1.5f, 5f);  // posición frente a la cámara

    void LateUpdate()
    {
        if (camara == null) return;

        // Pone el gizmo frente a la cámara con un offset local
        transform.position = camara.position + camara.TransformDirection(offsetPantalla);

        // Rota igual que la cámara
        transform.rotation = camara.rotation;
    }
}
