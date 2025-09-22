using UnityEngine;

public class CameraPitchLimiter : MonoBehaviour
{
    [Header("Límites del ángulo en X (Pitch)")]
    public float minPitch = -90f; // mirar abajo
    public float maxPitch = 90f;  // mirar arriba

    void LateUpdate()
    {
        Vector3 angles = transform.localEulerAngles;

        // Convertir ángulos de 0-360 → -180 a 180 (más fácil de clamplear)
        float pitch = angles.x;
        if (pitch > 180) pitch -= 360;

        // Limitar (clamp) el pitch
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // Reconstruir rotación con el pitch limitado
        transform.localEulerAngles = new Vector3(pitch, angles.y, 0f);
    }
}
