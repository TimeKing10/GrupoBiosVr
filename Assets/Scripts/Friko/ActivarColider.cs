using UnityEngine;

public class ActivarColliderPropio : MonoBehaviour
{
    private BoxCollider boxCollider;

    private void Awake()
    {
        // Busca el BoxCollider en este mismo objeto
        boxCollider = GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            Debug.LogWarning("No se encontró un BoxCollider en " + gameObject.name);
        }
    }

    /// <summary>
    /// Activa el BoxCollider de este objeto.
    /// </summary>
    public void Activar()
    {
        if (boxCollider != null)
        {
            boxCollider.enabled = true;
            Debug.Log("BoxCollider activado en " + gameObject.name);
        }
    }
}
