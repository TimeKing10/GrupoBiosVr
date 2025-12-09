using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors.Casters;

public class CurveRayAutoExtend : MonoBehaviour
{
    [Header("Distancias")]
    [Range(0.1f, 5f)] public float normalDistance = 0.3f;
    [Range(0.1f, 10f)] public float extendedDistance = 2f;

    [Header("Detección")]
    [Range(0.1f, 10f)] public float detectionRayDistance = 2f;
    public string targetTag = "Peligro";
    public Transform detectionOrigin;

    private CurveInteractionCaster curveCaster;
    private float currentDistance;

    void Start()
    {
        curveCaster = GetComponent<CurveInteractionCaster>();

        if (curveCaster == null)
        {
            Debug.LogError("Este script necesita un CurveInteractionCaster en el mismo objeto.");
            enabled = false;
            return;
        }

        if (detectionOrigin == null)
            detectionOrigin = curveCaster.castOrigin;

        // Inicializar distancia
        currentDistance = normalDistance;
        curveCaster.castDistance = normalDistance;
    }

    void Update()
    {
        bool shouldExtend = false;

        // 🔍 Solo detectar el tag
        if (Physics.Raycast(detectionOrigin.position,
                            detectionOrigin.forward,
                            out RaycastHit hit,
                            detectionRayDistance))
        {
            if (hit.collider.CompareTag(targetTag))
                shouldExtend = true;
        }

        // 🔁 Actualizar distancia del Ray
        float newDistance = shouldExtend ? extendedDistance : normalDistance;

        if (!Mathf.Approximately(currentDistance, newDistance))
        {
            currentDistance = newDistance;
            curveCaster.castDistance = currentDistance;
        }
    }

    private void OnDrawGizmos()
    {
        if (detectionOrigin == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(detectionOrigin.position,
                        detectionOrigin.position + detectionOrigin.forward * detectionRayDistance);
    }
}
