using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TeleportPlayerAlInicio : MonoBehaviour
{
    [Header("XR")]
    public Transform xrOrigin;      // XR Origin
    public Transform puntoInicio;   // Spawn

    [Header("Fade")]
    public RawImage fadeImage;
    public float fadeDuration = 1f;
    public float delayInicial = 1f; // ⏱ Espera al iniciar

    private IEnumerator Start()
    {
        if (xrOrigin == null || puntoInicio == null || fadeImage == null)
            yield break;

        // Asegurar que empiece en negro
        Color color = fadeImage.color;
        fadeImage.color = new Color(color.r, color.g, color.b, 1f);

        // Esperar a que XR inicialice (1 frame)
        yield return null;

        // Esperar 1 segundo después de iniciar
        yield return new WaitForSeconds(delayInicial);

        // Teleport al punto inicial
        xrOrigin.position = puntoInicio.position;
        xrOrigin.rotation = puntoInicio.rotation;

        // Fade de negro a transparente
        yield return StartCoroutine(Fade(1f, 0f));
    }

    IEnumerator Fade(float desde, float hasta)
    {
        float t = 0f;
        Color color = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(desde, hasta, t / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(color.r, color.g, color.b, hasta);
    }
}
