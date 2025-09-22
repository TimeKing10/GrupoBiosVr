using UnityEngine;

public class TrapTube : MonoBehaviour
{
    [Header("Waypoint del Jugador (opcional)")]
    public Transform playerWaypoint;  // puedes dejarlo si más adelante quieres usarlo

    [Header("Jugador")]
    public GameObject xrOrigin;       // XR Origin (jugador VR)
    public GameObject locomotion;     // Objeto locomotion a desactivar

    [Header("Sonidos")]
    public AudioSource grito;

    private bool isTrapped = false;

    public void Cogioeltubo()
    {
        // Reproducir sonido
        if (grito != null)
            grito.Play();

        // Desactivar locomotion para bloquear movimiento
        if (locomotion != null)
            locomotion.SetActive(false);

        isTrapped = true;
    }
}
