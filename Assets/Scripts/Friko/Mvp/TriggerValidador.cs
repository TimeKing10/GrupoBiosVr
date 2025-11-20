using UnityEngine;

public class TriggerValidador : MonoBehaviour
{
    public GameManagerXR manager;  // Asignar en el inspector

    private void OnTriggerEnter(Collider other)
    {
        // 1. Buscar si el objeto que entra tiene ContadorPadre
        ContadorPadre contador = other.GetComponent<ContadorPadre>();

        // 2. Si no lo tiene, salir sin hacer nada
        if (contador == null)
            return;

        // 3. Si lo tiene, evaluar
        if (contador.contador == 3)
        {
            // ➕ Suma paquete
            manager.SumarPaquete();
        }
        else
        {
            // ➖ Solo resta si hay puntos
            if (manager.paquetesEntregados > 0)
                manager.RestarPaquete();
        }
    }
}
