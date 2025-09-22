using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Explosion : MonoBehaviour
{
    [Header("Empuje")]
    public float fuerza = 20f;             // Qué tan fuerte lo manda
    public Vector3 direccionExtra = Vector3.up; // Extra hacia arriba

    [Header("Efectos")]
    public AudioSource sonido1;
    public AudioSource sonido2;
    public GameObject particulaExplosion; // Apagada en el inspector

    [Header("Fade UI")]
    public RawImage fadeImage;       // Pantalla negra (RawImage en Canvas)
    public float fadeDuration = 1f;  // Tiempo que tarda en aparecer el fade

    [Header("Cambio de escena")]
    public string nextSceneName;    // Nombre de la escena a cargar
    public float delayExtra = 0.5f; // Tiempo extra tras sonidos

    private bool activada = false;

    public void ActivarExplosion()
    {
        if (activada) return;
        activada = true;

        // 🔊 Reproducir sonidos
        if (sonido1 != null) sonido1.Play();
        if (sonido2 != null) sonido2.Play();

        // 💥 Encender la partícula
        if (particulaExplosion != null)
            particulaExplosion.SetActive(true);

        // 👉 Buscar al Player por Tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            CharacterController cc = player.GetComponent<CharacterController>();
            Rigidbody rb = player.GetComponent<Rigidbody>();
            Collider col = player.GetComponent<Collider>();

            if (cc != null && rb != null && col != null)
            {
                // Desactivar CharacterController
                cc.enabled = false;

                // Activar físicas
                rb.isKinematic = false;
                col.isTrigger = false;

                // Empuje desde la máquina hacia el jugador
                Vector3 direccion = (player.transform.position - transform.position).normalized;
                direccion += direccionExtra;
                rb.AddForce(direccion.normalized * fuerza, ForceMode.Impulse);
            }
        }

        // Fade progresivo
        if (fadeImage != null)
            StartCoroutine(FadeIn());

        // Cambio de escena después de sonidos
        StartCoroutine(LoadSceneAfterAudio());
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

        // Aseguramos negro total
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
