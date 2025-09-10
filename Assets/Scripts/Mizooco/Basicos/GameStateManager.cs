using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance; // Singleton global

    [Header("Riesgos / Estados")]
    public bool barrilDetectado;
    public bool charcoDetectado;
    public bool montacargasDetectado;
    public bool maquinaDetectada;

    private void Awake()
    {
        // Singleton: solo uno en toda la app
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // No se destruye entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        Debug.Log(barrilDetectado);
    }

    // Métodos de acceso (opcionales, para mayor claridad)
    public void SetBarril(bool value) => barrilDetectado = value;
    public void SetCharco(bool value) => charcoDetectado = value;
    public void SetMontacargas(bool value) => montacargasDetectado = value;
    public void SetMaquina(bool value) => maquinaDetectada = value;
}
