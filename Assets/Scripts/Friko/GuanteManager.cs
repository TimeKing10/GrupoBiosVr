using UnityEngine;

public class GuanteManager : MonoBehaviour
{
    [Header("Referencia al Trigger de guantes")]
    public GuanteTrigger guanteTrigger;  // Arrastra aquí el objeto que tiene GuanteTrigger

    [Header("Cambio de material")]
    public Renderer objetoRenderer;      // El objeto al que se le cambia el material
    public Material nuevoMaterial;       // Material que se aplicará

    /// <summary>
    /// Activa el guante izquierdo.
    /// </summary>
    public void ActivarGuanteIzquierdo()
    {
        if (guanteTrigger != null)
        {
            guanteTrigger.guanteIzquierdoPuesto = true;
            CambiarMaterial();
            gameObject.SetActive(false);
            Debug.Log("Guante izquierdo activado");
        }
    }

    /// <summary>
    /// Activa el guante derecho.
    /// </summary>
    public void ActivarGuanteDerecho()
    {
        if (guanteTrigger != null)
        {
            guanteTrigger.guanteDerechoPuesto = true;
            CambiarMaterial();
            gameObject.SetActive(false);
            Debug.Log("Guante derecho activado");
        }
    }

    /// <summary>
    /// Activa ambos guantes a la vez.
    /// </summary>
    public void ActivarAmbosGuantes()
    {
        if (guanteTrigger != null)
        {
            guanteTrigger.guanteIzquierdoPuesto = true;
            guanteTrigger.guanteDerechoPuesto = true;
            CambiarMaterial();
            Debug.Log("Ambos guantes activados");
        }
    }

    /// <summary>
    /// Cambia el material del objeto asignado.
    /// </summary>
    private void CambiarMaterial()
    {
        if (objetoRenderer != null && nuevoMaterial != null)
        {
            objetoRenderer.material = nuevoMaterial;
            Debug.Log("Material cambiado en " + objetoRenderer.gameObject.name);
        }
    }
}
