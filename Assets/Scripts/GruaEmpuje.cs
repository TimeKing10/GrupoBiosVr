using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GruaEmpuje : MonoBehaviour
{
    [Header("Empuje")]
    public float fuerza = 10f;

    [Header("Sonidos")]
    public AudioSource sonido1;
    public AudioSource sonido2;

    [Header("Fade UI")]
    public RawImage fadeImage;       // Pantalla negra (RawImage en Canvas)
    public float fadeDuration = 1f;  // Tiempo que tarda en aparecer el fade

    [Header("Cambio de escena")]
    public string nextSceneName;    // Nombre de la escena a cargar
    public float delayExtra = 0.5f; // Tiempo extra tras sonidos

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController cc = other.GetComponent<CharacterController>();
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (cc != null && rb != null)
            {
                // Apagar CharacterController y activar físicas
                cc.enabled = false;
                rb.isKinematic = false;

                // Dirección de empuje
                Vector3 direccion = (other.transform.position - transform.position).normalized;
                direccion.y = 0.5f; // un poco hacia arriba
                rb.AddForce(direccion * fuerza, ForceMode.Impulse);

                // 👉 IMPORTANTE: quitar el trigger de este collider
                Collider col = GetComponent<Collider>();
                if (col != null)
                    col.isTrigger = false;
            }

            // Reproducir sonidos
            if (sonido1 != null) sonido1.Play();
            if (sonido2 != null) sonido2.Play();

            // Fade progresivo
            if (fadeImage != null)
                StartCoroutine(FadeIn());

            // Cambio de escena
            StartCoroutine(LoadSceneAfterAudio());
        }
    }

    private IEnumerator FadeIn()
    {
        Color c = fadeImage.color;
        float alpha = c.a;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            fadeImage.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }

        // Aseguramos que queda en negro total
        fadeImage.color = new Color(c.r, c.g, c.b, 1f);
    }

    private IEnumerator LoadSceneAfterAudio()
    {
        float waitTime = delayExtra;

        // Esperar el sonido más largo
        float s1 = (sonido1 != null && sonido1.clip != null) ? sonido1.clip.length : 0f;
        float s2 = (sonido2 != null && sonido2.clip != null) ? sonido2.clip.length : 0f;
        waitTime += Mathf.Max(s1, s2);

        yield return new WaitForSeconds(waitTime);

        // Si escribiste el nombre en el inspector -> cargar esa escena
        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
