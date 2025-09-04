using UnityEngine;

public class VRPlayerCharacter : MonoBehaviour
{
    [Header("VR Settings")]
    public Transform cameraTransform; // Cámara VR (Main Camera del XR Origin)
    public float distancia = 0.5f;    // Distancia detrás de la cámara

    [Header("Límites de Altura")]
    public float alturaMin = 1.2f;    // Altura mínima permitida
    public float alturaMax = 2.0f;    // Altura máxima permitida

    void Start()
    {
        if (!cameraTransform)
        {
            Debug.LogError("⚠️ Asigna la cámara VR (Main Camera) en el inspector");
            enabled = false;
            return;
        }
    }

    void LateUpdate()
    {
        if (!cameraTransform) return;

        // Dirección hacia adelante de la cámara (solo en XZ)
        Vector3 forward = cameraTransform.forward;
        forward.y = 0;
        forward.Normalize();

        // Posición detrás de la cámara
        Vector3 newPosition;
        newPosition.x = cameraTransform.position.x - forward.x * distancia;
        newPosition.z = cameraTransform.position.z - forward.z * distancia;

        // Copiar la altura de la cámara pero limitada
        float altura = Mathf.Clamp(cameraTransform.position.y, alturaMin, alturaMax);
        newPosition.y = altura;

        // Aplicar posición
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
