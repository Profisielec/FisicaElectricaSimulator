using UnityEngine;

public class PunteroFrontal : MonoBehaviour
{
    public Transform camara;
    public float distancia = 3f;

    void Update()
    {
        if (camara == null) return;
        transform.position = camara.position + camara.forward * distancia;
        transform.rotation = Quaternion.LookRotation(camara.forward);
    }
}
