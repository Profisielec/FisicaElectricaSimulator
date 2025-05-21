using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SelectorDeParticulas : MonoBehaviour
{
    public Camera camara;
    public GameObject puntero2D;
    public GameObject puntero3D;
    public TextMeshProUGUI textoResultado;

    public TMP_InputField inputFieldCarga;
    public GameObject panelModificarCarga;
    public Button botonEliminar;
    public Button botonCancelarSeleccion;
    public Button botonCambiarCarga; // NUEVO

    private ElectricParticle particulaSeleccionada;
    private bool modoSeleccion = false;

    public void ActivarSeleccion()
    {
        modoSeleccion = true;

        if (puntero2D != null) puntero2D.SetActive(true);
        if (puntero3D != null) puntero3D.SetActive(false);
        if (textoResultado != null) textoResultado.text = "Toca una partícula...";
        if (panelModificarCarga != null) panelModificarCarga.SetActive(false);
        if (botonEliminar != null) botonEliminar.gameObject.SetActive(false);
        if (botonCancelarSeleccion != null) botonCancelarSeleccion.gameObject.SetActive(true);
        if (botonCambiarCarga != null) botonCambiarCarga.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!modoSeleccion || Input.touchCount == 0) return;

        Vector2 posicionPantalla = Input.GetTouch(0).position;
        Ray ray = camara.ScreenPointToRay(posicionPantalla);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            ElectricParticle p = hit.collider.GetComponent<ElectricParticle>();
            if (p != null)
            {
                particulaSeleccionada = p;
                MostrarInfo(p);

                modoSeleccion = false;
                if (puntero2D != null) puntero2D.SetActive(false);
                if (puntero3D != null) puntero3D.SetActive(true);
                if (panelModificarCarga != null) panelModificarCarga.SetActive(false);
                if (botonEliminar != null) botonEliminar.gameObject.SetActive(true);
                if (botonCancelarSeleccion != null) botonCancelarSeleccion.gameObject.SetActive(true);
                if (botonCambiarCarga != null) botonCambiarCarga.gameObject.SetActive(true);
            }
        }
    }

    void MostrarInfo(ElectricParticle p)
    {
        Vector3 fuerza = p.GetForce();
        float magnitud = fuerza.magnitude;
        float angulo = Mathf.Atan2(fuerza.z, fuerza.x) * Mathf.Rad2Deg;

        if (textoResultado != null)
            textoResultado.text = $"Carga: {p.charge:F2} C\nFuerza: {magnitud:F2} N\nÁngulo: {angulo:F1}°";

        if (inputFieldCarga != null)
            inputFieldCarga.text = p.charge.ToString("F2");
    }

    // ✅ LLAMADO DESDE EL BOTÓN "Cambiar carga"
    public void MostrarPanelModificarCarga()
    {
        if (particulaSeleccionada != null && panelModificarCarga != null)
        {
            panelModificarCarga.SetActive(true);
            if (botonCambiarCarga != null)
                botonCambiarCarga.gameObject.SetActive(false);
        }
    }

    // ✅ LLAMADO DESDE EL BOTÓN "Aplicar carga"
    public void AplicarNuevaCarga()
    {
        if (particulaSeleccionada != null && inputFieldCarga != null &&
            float.TryParse(inputFieldCarga.text, out float nuevaCarga))
        {
            particulaSeleccionada.SetCharge(nuevaCarga);
            MostrarInfo(particulaSeleccionada);

            if (panelModificarCarga != null) panelModificarCarga.SetActive(false);
            if (botonCambiarCarga != null) botonCambiarCarga.gameObject.SetActive(true);
        }
    }

    public void CancelarSeleccion()
    {
        particulaSeleccionada = null;
        if (textoResultado != null) textoResultado.text = "";
        if (panelModificarCarga != null) panelModificarCarga.SetActive(false);
        if (botonEliminar != null) botonEliminar.gameObject.SetActive(false);
        if (botonCancelarSeleccion != null) botonCancelarSeleccion.gameObject.SetActive(false);
        if (botonCambiarCarga != null) botonCambiarCarga.gameObject.SetActive(false);
    }

    public void EliminarParticulaSeleccionada()
    {
        if (particulaSeleccionada != null)
        {
            Destroy(particulaSeleccionada.gameObject);
            CancelarSeleccion();
        }
    }
}
