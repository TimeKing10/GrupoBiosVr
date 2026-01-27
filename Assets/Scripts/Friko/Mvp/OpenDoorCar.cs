using UnityEngine;

public class OpenDoorCar : MonoBehaviour
{
    public Animator puertar; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Carro"))
        {
            puertar.SetBool("Usar", true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Carro"))
        {
            puertar.SetBool("Usar", false);
        }
    }
}
