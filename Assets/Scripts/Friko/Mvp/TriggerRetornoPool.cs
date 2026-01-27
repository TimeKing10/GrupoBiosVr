using UnityEngine;

public class TriggerRetornoPool : MonoBehaviour
{
    public Transform puntoInicio;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("CajitaFalsa"))
            return;

        other.transform.position = puntoInicio.position;
        other.gameObject.SetActive(false);
    }
}