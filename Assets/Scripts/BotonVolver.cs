using UnityEngine;
using UnityEngine.SceneManagement;

public class BotonVolverMenu : MonoBehaviour
{
    public void VolverAlMenu()
    {
        SceneManager.LoadScene("MainScene");
    }
}
