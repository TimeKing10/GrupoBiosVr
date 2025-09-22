using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DesactivarPeligro : MonoBehaviour
{
    [Header("Fade UI")]
    public RawImage fadeImage;
    public float fadeInDuration = 0.3f;   // Tiempo para entrar a negro
    public float stayBlackDuration = 0.5f; // Tiempo que permanece en negro
    public float fadeOutDuration = 1f;   // Tiempo para volver a transparente

    [Header("Objetos a modificar")]
    public List<GameObject> targets = new List<GameObject>();
    public string newTag = "Untagged";

    [Header("Audio")]
    public AudioSource targetAudio;

    public void ActivarFade()
    {
        StartCoroutine(FadeAndChangeTags());
    }

    private IEnumerator FadeAndChangeTags()
    {
        // 1. Fade In (rápido a negro)
        yield return StartCoroutine(Fade(0f, 1f, fadeInDuration));

        // 2. Acción en medio
        foreach (GameObject obj in targets)
        {
            if (obj != null)
                obj.tag = newTag;
        }

        if (targetAudio != null)
            targetAudio.Play();

        // 3. Mantener negro
        yield return new WaitForSeconds(stayBlackDuration);

        // 4. Fade Out (más lento)
        yield return StartCoroutine(Fade(1f, 0f, fadeOutDuration));
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        float elapsed = 0f;
        Color c = fadeImage.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(from, to, elapsed / duration);
            c.a = alpha;
            fadeImage.color = c;
            yield return null;
        }

        c.a = to;
        fadeImage.color = c;
    }
}
