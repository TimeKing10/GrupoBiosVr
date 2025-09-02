using UnityEngine;

public class VRPlayerCharacter : MonoBehaviour
{
    public Transform cameraTransform; // Cámara VR
    public float distancia = 0.5f;    // Distancia detrás de la cámara

    void LateUpdate()
    {
        if (!cameraTransform) return;

        // Ángulo de inclinación (pitch)
        float pitch = cameraTransform.eulerAngles.x;
        if (pitch > 180) pitch -= 360; // Convertir a rango -180 a 180

        // Solo mover horizontal si pitch está entre -89 y 89
        if (pitch > -89f && pitch < 89f)
        {
            Vector3 forward = cameraTransform.forward;
            forward.y = 0;
            forward.Normalize();

            // Mover detrás de la cámara solo en X/Z
            Vector3 newPosition = transform.position;
            newPosition.x = cameraTransform.position.x - forward.x * distancia;
            newPosition.z = cameraTransform.position.z - forward.z * distancia;

            // Mantener altura fija
            newPosition.y = transform.position.y;

            transform.position = newPosition;
        }

        // Rotación solo en Y siguiendo la cámara
        Vector3 lookDir = cameraTransform.forward;
        lookDir.y = 0; // ignorar X/Z
        if (lookDir.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(lookDir, Vector3.up);
        }
    }
}
