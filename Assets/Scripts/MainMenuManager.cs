using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void CargarJuego1()
    {
        SceneManager.LoadScene("SelectorModo");
    }

    public void CargarJuego2()
    {
        SceneManager.LoadScene("Juego2");
    }

    public void CargarJuego3()
    {
        SceneManager.LoadScene("Juego3");
    }
    public void CargarAleatorio()
    {
        SceneManager.LoadScene("Juego1"); // Simulador Aleatorio
    }
    public void CargarMenu()
    {
        SceneManager.LoadScene("MainScene"); 
    }
    public void CargarManual()
    {
        SceneManager.LoadScene("SimuladorManual"); // Simulador Manual
    }
}