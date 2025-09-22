using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class BarrelTrigger : MonoBehaviour
{
    [Header("Fade UI")]
    public RawImage fadeImage;      // La RawImage negra para el fade

    [Header("Audio")]
    public AudioSource audioSource; // El AudioSource que tiene el golpe
    public AudioSource Grito;    
    public float gritoDelay = 0.5f; // ⏳ Retraso del grito

    [Header("Cambio de escena")]
    public string nextSceneName;    // Nombre de la escena a cargar (configurable en el inspector)

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Sonido de golpe inmediato
            if (audioSource != null)
                audioSource.Play();

            // Grito después de un pequeño delay
            if (Grito != null)
                Grito.PlayDelayed(gritoDelay);

            // Pantalla negra instantánea
            if (fadeImage != null)
            {
                Color c = fadeImage.color;
                c.a = 1f;
                fadeImage.color = c;
            }

            // Iniciar cambio de escena cuando termine el grito
            StartCoroutine(LoadSceneAfterAudio());
        }
    }

    private IEnumerator LoadSceneAfterAudio()
    {
        float waitTime = 1f; // tiempo mínimo en caso de no haber grito

        if (Grito != null && Grito.clip != null)
        {
            // esperar: delay + duración del grito
            waitTime = gritoDelay + Grito.clip.length;
        }

        yield return new WaitForSeconds(waitTime);

        // Si escribiste el nombre en el inspector -> cargar esa escena
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            // Fallback: si no se asigna, recarga la misma escena
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
