using UnityEngine;

public class FinalizarTuto : MonoBehaviour
{
    [Header("Objetos a activar")]
    public GameObject objetoAPrender;
    public GameObject objetoAPrender2;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Canasta"))
        {
            Debug.Log("Canasta entró en el tutorial");
            if (objetoAPrender != null)
                objetoAPrender.SetActive(true);
            if (objetoAPrender2 != null)
                objetoAPrender2.SetActive(true);    
        }
    }
}
