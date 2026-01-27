using UnityEngine;
using TMPro;

public class TriggerValidador : MonoBehaviour
{
    public GameManagerXR manager;

    [Header("Zonas de Teletransporte")]
    public Transform zonaCanasta;

    public TextMeshProUGUI textoContador;

    private void OnTriggerEnter(Collider other)
    {
        // 🔹 El trigger se activa por el Contador
        if (!other.CompareTag("Contador"))
            return;

        Debug.Log("Contador entró en la zona");

        ContadorPaquetes contador = other.GetComponentInParent<ContadorPaquetes>();
        if (contador == null)
        {
            Debug.LogWarning("No se encontró ContadorPaquetes");
            return;
        }

        // 🔢 Solo sumar si está completa (7/7)
        if (contador.cantidad >= 7)
        {
            manager.SumarPaquete();
            textoContador.text = "0/7";
            Debug.Log("Canasta válida, se sumó puntaje");
        }

        // 🔄 Resetear visuales
        contador.ResetearCanastaVisual();

        // 🎯 BUSCAR LA CANASTA REAL
        Transform canasta = BuscarCanasta(other.transform);
        if (canasta == null)
        {
            Debug.LogError("No se encontró objeto con tag Canasta");
            return;
        }

        // 🚚 Teletransportar SOLO la canasta
        Rigidbody rb = canasta.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        canasta.position = zonaCanasta.position;
        canasta.rotation = zonaCanasta.rotation;
    }

    private Transform BuscarCanasta(Transform inicio)
    {
        Transform actual = inicio;

        while (actual != null)
        {
            if (actual.CompareTag("Canasta"))
                return actual;

            actual = actual.parent;
        }

        return null;
    }
}
