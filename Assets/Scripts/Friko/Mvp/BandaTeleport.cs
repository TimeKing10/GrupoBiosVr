using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BandaTeleport : MonoBehaviour
{
    [Header("XR")]
    public Transform xrOrigin;          // XR Origin
    public Transform puntoTeleport;     // Punto destino

    [Header("Audio")]
    public AudioSource audio2;

    [Header("Fade")]
    public RawImage fadeImage;
    public float fadeDuration = 1f;

    private bool activado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activado) return;

        if (other.CompareTag("LeftHand") || other.CompareTag("RightHand"))
        {
            activado = true;

            // Audio
            if (audio2 != null)
                audio2.Play();

            // Fade + Teleport
            StartCoroutine(FadeAndTeleport());
        }
    }

    private IEnumerator FadeAndTeleport()
    {
        // Fade a negro
        yield return StartCoroutine(Fade(0f, 1f));

        // Pequeña espera para VR
        yield return null;

        // Teleport
        if (xrOrigin != null && puntoTeleport != null)
        {
            xrOrigin.position = puntoTeleport.position;
            xrOrigin.rotation = puntoTeleport.rotation;
        }

        yield return new WaitForSeconds(0.1f);

        // Fade de regreso
        yield return StartCoroutine(Fade(1f, 0f));

        activado = false;
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
