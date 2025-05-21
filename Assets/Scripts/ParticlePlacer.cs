using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ParticlePlacer : MonoBehaviour
{
    public GameObject particulaPrefab;
    public Transform puntero;
    public Button btnColocar;
    public Button btnAlternar;

    private bool cargaPositiva = true;
    private Stack<GameObject> historial = new Stack<GameObject>();
    private bool esperandoClick = false;

    void Start()
    {
        btnColocar.onClick.AddListener(PrepararColocacion);
        btnAlternar.onClick.AddListener(AlternarCarga);
        ActualizarColorPuntero();
    }

    void Update()
    {
        if (esperandoClick && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            ColocarParticula();
        }
    }

    public void PrepararColocacion()
    {
        esperandoClick = true;
    }

    public void ColocarParticula()
    {
        GameObject nueva = Instantiate(particulaPrefab, puntero.position, Quaternion.identity);
        ElectricParticle ep = nueva.GetComponent<ElectricParticle>();

        if (ep != null)
        {
            ep.SetCharge(cargaPositiva ? 1f : -1f);
            ep.isStatic = true;
        }

        historial.Push(nueva);
        esperandoClick = false;
    }

    public void AlternarCarga()
    {
        cargaPositiva = !cargaPositiva;
        ActualizarColorPuntero();
    }

    void ActualizarColorPuntero()
    {
        Renderer r = puntero.GetComponent<Renderer>();
        if (r != null)
        {
            r.material.color = cargaPositiva ? Color.red : Color.blue;
        }
    }

    public void CancelarColocacion()
    {
        esperandoClick = false;
    }

    public void ReactivarColocacion()
    {
        esperandoClick = true;
    }
}
