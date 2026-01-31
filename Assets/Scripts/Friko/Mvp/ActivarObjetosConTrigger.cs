using UnityEngine;

public class ActivarDesactivarObjetosConTrigger : MonoBehaviour
{
    [Header("Objetos a activar")]
    public GameObject[] objetosAActivar;

    [Header("Objetos a desactivar")]
    public GameObject[] objetosADesactivar;

    [Header("Tag del Player")]
    public string tagPlayer = "Player";

    private bool yaActivado = false;

    private void OnTriggerEnter(Collider other)
    {
        // Evita que se ejecute más de una vez
        if (yaActivado)
            return;

        // Verifica que sea el jugador
        if (!other.CompareTag(tagPlayer))
            return;

        yaActivado = true;

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