using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BandaTransportadora : MonoBehaviour
{
    [Header("Configuración de la banda")]
    public float velocidad = 2f;

    [Header("Filtro de objetos")]
    public string layerAlimento = "AlimentoMizooco"; // Nombre del Layer
    public string tagAlimento = "Alimento";         // Tag requerido

    private int alimentoLayer;

    void Start()
    {
        // Convertir nombre de layer a índice
        alimentoLayer = LayerMask.NameToLayer(layerAlimento);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Colisión detectada con: " + collision.gameObject.name);

    }
    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("Colisión detectada con: " + collision.gameObject.name);
        GameObject obj = collision.gameObject;

        // Verificar si cumple una de las dos condiciones (tag O layer)
        if (obj.CompareTag(tagAlimento) || obj.layer == alimentoLayer)
        {
            Rigidbody rb = collision.rigidbody;
            if (rb != null)
            {
                rb.linearVelocity = transform.forward * velocidad;
            }
        }
    }
}
