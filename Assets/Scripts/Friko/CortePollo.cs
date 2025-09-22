using UnityEngine;

public class CortePollo : MonoBehaviour
{
    [Header("Partes de la pechuga a liberar (puedes poner 1 o 2)")]
    public GameObject partePechuga1; 
    public GameObject partePechuga2;

    [Header("Audio")]
    public AudioSource sonidoCorte; // arrastra aquí el AudioSource con el sonido

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cuchillo"))
        {
            Debug.Log("Entro el cuchillo");

            // Reproducir sonido de corte
            if (sonidoCorte != null)
                sonidoCorte.Play();

            // Liberar parte 1 si existe
            LiberarParte(partePechuga1);

            // Liberar parte 2 si existe
            LiberarParte(partePechuga2);

            // Desactivar este objeto (trigger de corte)
            gameObject.SetActive(false);
        }
    }

    private void LiberarParte(GameObject parte)
    {
        if (parte != null)
        {
            // Quitar del padre
            parte.transform.SetParent(null);

            // Activar Rigidbody
            Rigidbody rb = parte.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }

            // Activar Collider
            Collider col = parte.GetComponent<Collider>();
            if (col != null)
            {
                col.enabled = true;
            }
        }
    }
}
