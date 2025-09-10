using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");

        // Si estamos en el editor, detener play mode
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Si es una build real, cerrar la app
        Application.Quit();
#endif
    }
}
