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

            // Iniciar recarga cuando termine el grito (o un fallback si no hay grito)
            StartCoroutine(ReloadAfterAudio());
        }
    }

    private IEnumerator ReloadAfterAudio()
    {
        float waitTime = 1f; // tiempo mínimo en caso de no haber grito

        if (Grito != null && Grito.clip != null)
        {
            // esperar: delay + duración del grito
            waitTime = gritoDelay + Grito.clip.length;
        }

        yield return new WaitForSeconds(waitTime);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
