using UnityEngine;
using System.Collections.Generic;

public class CajaReceptora : MonoBehaviour
{
    [Header("Capacidad máxima de la caja")]
    public int capacidadMaxima = 30;

    // Cola FIFO
    private Queue<GameObject> alimentosEnCaja = new Queue<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        // ✅ Condición: que tenga tag "Bolsa" o que esté en la layer "AlimentoMizooco"
        if (other.CompareTag("Bolsa") || other.gameObject.layer == LayerMask.NameToLayer("AlimentoMizooco"))
        {
            // Guardar el alimento en la cola
            alimentosEnCaja.Enqueue(other.gameObject);

            // Si pasa el límite, destruir el más viejo
            if (alimentosEnCaja.Count > capacidadMaxima)
            {
                GameObject masViejo = alimentosEnCaja.Dequeue();
                if (masViejo != null)
                {
                    Destroy(masViejo);
                }
            }

        }
    }
}
