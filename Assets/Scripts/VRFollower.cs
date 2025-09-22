using UnityEngine;

public class VRFollower : MonoBehaviour
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

        // Dirección hacia adelante de la cámara con su rotación completa
        Vector3 backward = -cameraTransform.forward;

        // Posición detrás de la cámara
        Vector3 newPosition = cameraTransform.position + backward * distancia;

        // Copiar la altura de la cámara pero limitada
        float altura = Mathf.Clamp(cameraTransform.position.y, alturaMin, alturaMax);
        newPosition.y = altura;

        // Aplicar posición
        transform.position = newPosition;

        // 👉 Seguir rotación completa de la cámara (X, Y, Z)
        transform.rotation = cameraTransform.rotation;
    }
}
