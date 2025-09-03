using UnityEngine;

public class VRPlayerCharacter : MonoBehaviour
{
    [Header("VR Settings")]
    public Transform cameraTransform; // Cámara VR (Main Camera del XR Origin)
    public float distancia = 0.5f;    // Distancia detrás de la cámara
    public float tiempoCalibracion = 3f; // Tiempo que seguirá la altura de la cámara

    private float alturaFija;     
    private float tiempoTranscurrido; 
    private bool alturaBloqueada = false;

    void Start()
    {
        if (!cameraTransform)
        {
            Debug.LogError("⚠️ Asigna la cámara VR (Main Camera) en el inspector");
            enabled = false;
            return;
        }

        // Arrancamos con la altura inicial de la cámara
        alturaFija = cameraTransform.position.y;
    }

    void LateUpdate()
    {
        if (!cameraTransform) return;

        tiempoTranscurrido += Time.deltaTime;

        // Durante los primeros segundos, seguimos la altura de la cámara
        if (!alturaBloqueada)
        {
            alturaFija = cameraTransform.position.y;

            if (tiempoTranscurrido >= tiempoCalibracion)
                alturaBloqueada = true; // Después de X segundos, la fijamos
        }

        // Dirección hacia adelante de la cámara (solo en XZ)
        Vector3 forward = cameraTransform.forward;
        forward.y = 0;
        forward.Normalize();

        // Posición detrás de la cámara
        Vector3 newPosition;
        newPosition.x = cameraTransform.position.x - forward.x * distancia;
        newPosition.z = cameraTransform.position.z - forward.z * distancia;

        // Usar la altura (sea dinámica al inicio o fija después)
        newPosition.y = alturaFija;

        transform.position = newPosition;

        // Rotar solo en Y siguiendo la cámara
        Vector3 lookDir = cameraTransform.forward;
        lookDir.y = 0;
        if (lookDir.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(lookDir, Vector3.up);
        }
    }
}
