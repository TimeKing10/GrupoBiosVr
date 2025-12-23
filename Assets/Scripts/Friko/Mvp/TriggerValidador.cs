using UnityEngine;

public class TriggerValidador : MonoBehaviour
{
    public GameManagerXR manager;

    [Header("Zonas de Teletransporte")]
    public Transform zonaCanasta;
    public Transform zonaDescarte;

    private void OnTriggerEnter(Collider other)
    {
        // 🔹 SI ES CANASTA
        if (other.CompareTag("Contador"))
        {
            ContadorPaquetes contador = other.GetComponent<ContadorPaquetes>();

            if (contador != null && contador.cantidad > 0)
            {
                Debug.Log("Se sumo");
                manager.SumarPaquete();
                
            }

            // 🚚 Teletransportar canasta
            // 🔄 Resetear paquetes visuales
            contador.ResetearCanastaVisual();

            return;
        }
        if (other.CompareTag("Canasta"))
        {
            other.transform.position = zonaCanasta.position;
            other.transform.rotation = zonaCanasta.rotation;
        }

        // 🔹 NO tocar otros objetos aquí
    }
}
