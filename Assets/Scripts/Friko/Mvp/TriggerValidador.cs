using UnityEngine;
using TMPro;
public class TriggerValidador : MonoBehaviour
{
    public GameManagerXR manager;

    [Header("Zonas de Teletransporte")]
    public Transform zonaCanasta;
    public Transform zonaDescarte;
    public TextMeshProUGUI textoContador;

    private void OnTriggerEnter(Collider other)
    {
        // 🔹 SI ES CANASTA
        if (other.CompareTag("Contador"))
        {
            ContadorPaquetes contador = other.GetComponent<ContadorPaquetes>();

            if (contador != null && contador.cantidad >6)
            {
                textoContador.text = "0/7";
                Debug.Log("Se sumo");
                manager.SumarPaquete();
                
            }

            // 🚚 Teletransportar canasta
            // 🔄 Resetear paquetes visuales
            other.transform.position = zonaCanasta.position;
            other.transform.rotation = zonaCanasta.rotation;
            contador.ResetearCanastaVisual();

            return;
        }
        

        // 🔹 NO tocar otros objetos aquí
    }
}
