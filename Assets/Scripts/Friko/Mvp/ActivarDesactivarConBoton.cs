using UnityEngine;

public class ActivarDesactivarConBoton : MonoBehaviour
{
    [Header("Objetos a activar")]
    public GameObject[] objetosAActivar;

    [Header("Objetos a desactivar")]
    public GameObject[] objetosADesactivar;

    private bool yaUsado = false;

    // Llamar esta función desde el botón
    public void EjecutarAccion()
    {
        // Evita que se ejecute más de una vez
        if (yaUsado)
            return;

        yaUsado = true;

        // ✅ Activar objetos
        foreach (GameObject obj in objetosAActivar)
        {
            if (obj != null)
                obj.SetActive(true);
        }

        // ❌ Desactivar objetos
        foreach (GameObject obj in objetosADesactivar)
        {
            if (obj != null)
                obj.SetActive(false);
        }
    }
}