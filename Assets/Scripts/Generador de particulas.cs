using UnityEngine;
using TMPro;

public class GeneradorDeParticulas : MonoBehaviour
{
    public GameObject particulaPositivaPrefab;
    public GameObject particulaNegativaPrefab;

    public GameObject panelUI;
    public TextMeshProUGUI textoTipoParticula;

    private bool crearPositiva = true;
    private bool esperandoClick = false;
    private float cargaTemporal = 1f;

    void Start()
    {
        ActualizarTextoTipo();
    }

    void Update()
    {
        // Esperar clic para colocar la partícula
        if (esperandoClick && Input.GetMouseButtonDown(0))
        {
            Vector2 posicion = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            GameObject prefab = crearPositiva ? particulaPositivaPrefab : particulaNegativaPrefab;
            GameObject nueva = Instantiate(prefab, posicion, Quaternion.identity);

            // Quitar tag "BaseParticula" si lo tenía antes
            nueva.tag = "Untagged";

            ParticulaConCarga scriptCarga = nueva.GetComponent<ParticulaConCarga>();
            if (scriptCarga != null)
            {
                scriptCarga.Inicializar(cargaTemporal);
            }

            esperandoClick = false;
        }
    }

    // Este método se llama desde el botón "Agregar Partícula"
    public void MostrarPanelParticula()
    {
        panelUI.SetActive(true);
    }

    // Ahora toma la carga directamente desde el UIManager
    public void ConfirmarMagnitud()
    {
        float magnitud = UIManager.instance.ObtenerCargaActual();

        cargaTemporal = magnitud;
        esperandoClick = true;

        panelUI.SetActive(false);

        Debug.Log("Haz clic para colocar la partícula con magnitud: " + cargaTemporal);
    }

    public void AlternarTipo()
    {
        crearPositiva = !crearPositiva;
        ActualizarTextoTipo();
    }

    private void ActualizarTextoTipo()
    {
        if (textoTipoParticula != null)
        {
            textoTipoParticula.text = crearPositiva ? "Tipo actual: POSITIVA" : "Tipo actual: NEGATIVA";
        }
    }
}
