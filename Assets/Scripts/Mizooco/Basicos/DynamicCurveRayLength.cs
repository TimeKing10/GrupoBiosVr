using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors.Casters;

public class DynamicCurveRayLength : MonoBehaviour
{
    [Header("Distancias del CurveCaster")]
    [Range(0.1f, 5f)] public float normalDistance = 0.3f;
    [Range(0.1f, 10f)] public float extendedDistance = 2f;

    [Header("Detección con Raycast")]
    [Range(0.1f, 10f)] public float detectionRayDistance = 2f; 
    public string targetTag = "Peligro"; 

    [Header("Ray de Detección")]
    public Transform detectionOrigin;

    [Header("Materiales")]
    public Material dangerMaterial; // 👈 material rojo para resaltar peligro

    private CurveInteractionCaster curveCaster;
    private float currentDistance;

    private Renderer lastRenderer;   // último objeto apuntado
    private Material originalMaterial; // material original del objeto

    void Start()
    {
        curveCaster = GetComponent<CurveInteractionCaster>();
        if (curveCaster == null)
        {
            Debug.LogError("Este script requiere un CurveInteractionCaster en el mismo objeto.");
            enabled = false;
            return;
        }

        if (detectionOrigin == null)
            detectionOrigin = curveCaster.castOrigin;

        currentDistance = normalDistance;
        curveCaster.castDistance = normalDistance;
    }

    void Update()
    {
        bool shouldExtend = false;
        GameObject hitObject = null;

        // 🔍 rayo de detección
        if (Physics.Raycast(detectionOrigin.position,
                            detectionOrigin.forward,
                            out RaycastHit hit,
                            detectionRayDistance))
        {
            if (hit.collider.CompareTag(targetTag))
            {
                shouldExtend = true;
                hitObject = hit.collider.gameObject;
            }
        }

        // Cambiar longitud del curveCaster
        SetRayDistance(shouldExtend ? extendedDistance : normalDistance);

        // 🔴 manejo de materiales
        HandleHighlight(hitObject);
    }

    void SetRayDistance(float distance)
    {
        if (Mathf.Approximately(currentDistance, distance))
            return;

        currentDistance = distance;
        curveCaster.castDistance = currentDistance;
    }

    void HandleHighlight(GameObject hitObject)
    {
        // Si seguimos apuntando al mismo objeto, no hacemos nada
        if (hitObject != null && lastRenderer != null && hitObject == lastRenderer.gameObject)
            return;

        // Si dejamos de apuntar al último objeto -> restaurar material
        if (lastRenderer != null)
        {
            lastRenderer.material = originalMaterial;
            lastRenderer = null;
            originalMaterial = null;
        }

        // Si estamos apuntando a un nuevo objeto con tag peligro -> cambiar a rojo
        if (hitObject != null)
        {
            Renderer rend = hitObject.GetComponent<Renderer>();
            if (rend != null)
            {
                lastRenderer = rend;
                originalMaterial = rend.material;
                rend.material = dangerMaterial;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (detectionOrigin == null) return;

        Gizmos.color = Color.yellow;
        Vector3 endPoint = detectionOrigin.position + detectionOrigin.forward * detectionRayDistance;
        Gizmos.DrawLine(detectionOrigin.position, endPoint);

        if (Physics.Raycast(detectionOrigin.position, detectionOrigin.forward,
                            out RaycastHit hit, detectionRayDistance))
        {
            if (hit.collider.CompareTag(targetTag))
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(hit.point, 0.05f);
            }
        }
    }
}
