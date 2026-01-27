using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ReinicioPorColision : MonoBehaviour
{
    [Header("Audio")]
    
    public AudioSource audio2;

    [Header("Fade")]
    public RawImage fadeImage;
    public float fadeDuration = 1f;


    [Header("Escena")]
    public string nombreEscena; // Déjalo vacío para recargar la actual

    private bool activado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!activado && other.CompareTag("LeftHand") || other.CompareTag("RightHand"))
        {
            activado = true;

            // Reproducir ambos sonidos
            if (audio2 != null) audio2.Play();
            
            

            // Iniciar Fade y luego reinicio
            StartCoroutine(FadeAndRestart());
        }
    }

    private IEnumerator FadeAndRestart()
    {
        Color color = fadeImage.color;
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Clamp01(t / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        // Reiniciar escena
        if (!string.IsNullOrEmpty(nombreEscena))
            SceneManager.LoadScene(nombreEscena);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
