using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BandaTransportadora : MonoBehaviour
{
    public float velocidad = 2f;
    public string layerAlimento = "AlimentoMizooco"; // Nombre del Layer

    private int alimentoLayer;

    void Start()
    {
        // Convertir nombre de layer a índice
        alimentoLayer = LayerMask.NameToLayer(layerAlimento);
    }

    private void OnCollisionStay(Collision collision)
    {
        // Solo mover si está en el layer correcto
        if (collision.gameObject.layer == alimentoLayer)
        {
            Rigidbody rb = collision.rigidbody;
            if (rb != null)
            {
                rb.linearVelocity = transform.forward * velocidad;
            }
        }
    }
}
