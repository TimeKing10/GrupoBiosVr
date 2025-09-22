using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GuanteTrigger : MonoBehaviour
{
    [Header("Estado de guantes")]
    public bool guanteIzquierdoPuesto = false;   // True si el jugador tiene el guante izquierdo
    public bool guanteDerechoPuesto = false;     // True si el jugador tiene el guante derecho

    [Header("UI")]
    public RawImage rawImage;       // Imagen que se activa al colisionar
    public RawImage fadeImage;      // Imagen usada para el fade
    public float fadeDuration = 1.5f;

    [Header("Audio")]
    public AudioSource sonido;      // Sonido que se reproduce

    [Header("Escena")]
    public string nombreEscena;     // Nombre de la escena a cargar

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        // Mano izquierda con guante
        if (other.CompareTag("LeftHand") && !guanteIzquierdoPuesto)
        {
            ActivarEvento();
        }

        // Mano derecha con guante
        if (other.CompareTag("RightHand") && !guanteDerechoPuesto)
        {
            ActivarEvento();
        }
    }

    private void ActivarEvento()
    {
        triggered = true;

        if (rawImage != null) rawImage.gameObject.SetActive(true);
        // Reproducir sonido
        if (sonido != null) sonido.Play();

        // Mostrar imagen
        

        // Iniciar fade y cambio de escena
        StartCoroutine(FadeAndChangeScene());
    }

    private IEnumerator FadeAndChangeScene()
    {
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;

            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                c.a = Mathf.Clamp01(elapsed / fadeDuration);
                fadeImage.color = c;
                yield return null;
            }
        }

        if (!string.IsNullOrEmpty(nombreEscena))
        {
            SceneManager.LoadScene(nombreEscena);
        }
    }
}
