using UnityEngine;

public class TutoBanda : MonoBehaviour
{ 
    [Header("Objetos a activar")]
    public GameObject objetoAPrender;
    public GameObject objetoAPrender2;

     void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {   if (objetoAPrender != null)
            objetoAPrender.SetActive(true);
            if (objetoAPrender2 != null)
                objetoAPrender2.SetActive(true); 
            Destroy(gameObject);    
        }

        
    }
}
