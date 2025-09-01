using UnityEngine;
using UnityEngine.Playables; // Necesario para controlar Timeline

public class TimelineLoop : MonoBehaviour
{
    [Header("Configuración")]
    public float velocidad = 1f; // 1 = normal, 2 = el doble de rápido, 0.5 = mitad de velocidad

    private PlayableDirector director;
    private bool activo = false; // Estado de la timeline (privado)

    void Start()
    {
        director = GetComponent<PlayableDirector>();

        // Asegurarse que la Timeline esté detenida y en el inicio
        director.Stop();
        director.time = 0;

        // Evitamos Evaluate en Start para no disparar Signals
        director.stopped += OnTimelineEnd;
    }

    void Update()
    {
        if (activo)
        {
            director.playableGraph.GetRootPlayable(0).SetSpeed(velocidad);
        }
    }

    void OnTimelineEnd(PlayableDirector d)
    {
        if (activo) // Solo repetir si está encendida
        {
            director.time = 0;
            director.Play();
        }
    }

    // --- Métodos de control ---
    public void Encender()
    {
        if (!activo)
        {
            activo = true;
            director.time = 0;
            director.Play();
        }
    }

    public void Apagar()
    {
        if (activo)
        {
            activo = false;
            director.Stop();
            director.time = 0;
            director.Evaluate(); // ✅ resetea visualmente
        }
    }

    public void Alternar()
    {
        if (activo)
            Apagar();
        else
            Encender();
    }

    // --- Método para que el Manager cambie el bool ---
    public void SetActivo(bool valor)
    {
        activo = valor;
    }

    // --- Getter si necesitas saber desde UI si está encendida ---
    public bool GetActivo()
    {
        return activo;
    }
}
