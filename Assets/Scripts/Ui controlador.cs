using UnityEngine;
using TMPro;                         // ← para TextMesh Pro

public class UIControlador : MonoBehaviour
{
    [Header("Referencias de UI")]
    public GameObject panelConfiguracion;   // Panel que quieres ocultar
    public TMP_InputField inputPositivas;   // Campo “P. positivas”
    public TMP_InputField inputNegativas;   // Campo “P. negativas”

    [Header("Generador de partículas")]
    public ParticleSystemGenerator generador;   // Arrastra aquí tu objeto con el script de partículas

    void Start()
    {
        // Asegúrate de que el panel comience visible
        panelConfiguracion.SetActive(true);

        // (Opcional) Pausa la simulación hasta que el jugador la inicie
        // Time.timeScale = 0f;
    }

    /// <summary>
    /// Se llama desde el botón “Iniciar simulación”.
    /// </summary>
    public void IniciarSimulacion()
    {
        // --- 1. Leer los valores escritos por el jugador ---
        int positivas = int.Parse(inputPositivas.text);
        int negativas = int.Parse(inputNegativas.text);

        // --- 2. Pasarlos al generador ---
        generador.numPositiveParticles = positivas;
        generador.numNegativeParticles = negativas;

        // --- 3. Ocultar el panel ---
        panelConfiguracion.SetActive(false);

        // --- 4. Iniciar la creación de partículas ---
        generador.EmpezarGeneracion();

        // --- 5. Reanudar la simulación (si estaba en pausa) ---
        // Time.timeScale = 1f;
    }
}
