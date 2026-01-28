using UnityEngine;
using System.Collections;

public class ActivarObjetosConDelay : MonoBehaviour
{
    [Header("Tiempo de espera")]
    public float delay = 5f;

    [Header("Objetos a activar")]
    public GameObject[] objetosAActivar;

    [Header("Audio")]
    public AudioSource audioParaApagar;

    private void Start()
    {
        StartCoroutine(ActivarDespuesDeTiempo());
    }

    private IEnumerator ActivarDespuesDeTiempo()
    {
        yield return new WaitForSeconds(delay);

        // 🔇 Apagar audio
        if (audioParaApagar != null)
            audioParaApagar.Stop();

        // ✅ Activar objetos
        foreach (GameObject obj in objetosAActivar)
        {
            if (obj != null)
                obj.SetActive(true);
        }
    }
}