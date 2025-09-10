using UnityEngine;
using UnityEngine.SceneManagement;

public class ContadorManager : MonoBehaviour
{
    public static ContadorManager Instance; 

    [Header("Contador de Riesgos")]
    [Range(0, 4)]
    public int contador = 0;  // Va de 0 a 4

    void Awake()
    {
        // Configuración del Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persiste entre escenas
        }
        else
        {
            Destroy(gameObject); // Evita duplicados
        }
    }

    void OnEnable()
    {
        // Cuando cargues la escena del contador, lo reinicia
        SceneManager.sceneLoaded += ReiniciarContador;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= ReiniciarContador;
    }

    private void ReiniciarContador(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "EscenaContador") // 🔹 Cambia por el nombre de tu escena
        {
            contador = 0;
        }
    }

    // 🔹 Método para sumar al contador
    public void SumarEvento()
    {
        if (contador < 4)
            contador++;
    }

    // 🔹 Método para obtener el valor
    public int GetContador()
    {
        return contador;
    }
}
