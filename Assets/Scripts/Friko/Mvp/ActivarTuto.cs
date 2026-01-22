using UnityEngine;

public class ActivarTuto : MonoBehaviour
{
    [Header("Objetos a activar")]
    public GameObject objetoAPrender;
    public GameObject objetoAPrender2;

    

    private void OnCollisionEnter(Collision collision)
    {
        // Buscar ContadorPadre en el objeto que colisiona
        ContadorPadre contador = collision.gameObject.GetComponent<ContadorPadre>();

        if (contador == null)
            return;

        // Validar si está completo
        if (contador.contador == contador.maxContador)
        {
            if (objetoAPrender != null)
                objetoAPrender.SetActive(true);
            if (objetoAPrender2 != null)
                objetoAPrender2.SetActive(true);    
        }
    }
   
}
