using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiarEscena : MonoBehaviour
{
    [Header("Nombre de la escena a cargar")]
    public string nombreEscena;

    // Llama a este método desde el botón
    public void CargarEscena()
    {
        if (!string.IsNullOrEmpty(nombreEscena))
        {
            SceneManager.LoadScene(nombreEscena);
        }
        else
        {
            Debug.LogWarning("No se ha asignado un nombre de escena en el inspector.");
        }
    }
}
