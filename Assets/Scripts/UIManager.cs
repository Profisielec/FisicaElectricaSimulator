using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public TMP_Text fuerzaText;
    public TMP_InputField campoCargaInput;

    private float cargaActual = 1f;

    void Awake()
    {
        instance = this;
    }

    public void ActualizarCargaAutomatica(string texto)
    {
        if (float.TryParse(texto, out float nuevaCarga))
        {
            cargaActual = nuevaCarga;
            Debug.Log("Carga seleccionada actualizada automáticamente: " + cargaActual + " μC");
        }
        else
        {
            Debug.LogWarning("Valor inválido ingresado automáticamente.");
        }
    }

    public float ObtenerCargaActual()
    {
        return cargaActual;
    }

    public void MostrarFuerza(float fuerza)
    {
        if (fuerzaText != null)
            fuerzaText.text = "Fuerza: " + fuerza.ToString("F2") + " N";
    }
}
