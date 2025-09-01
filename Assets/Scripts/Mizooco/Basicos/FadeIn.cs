using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeIn : MonoBehaviour
{
    [Header("Fade UI")]
    public RawImage fadeImage;       // Asigna la RawImage desde el inspector
    public float duracion = 2f;      // Tiempo que tarda el fade en desaparecer

    private void Start()
    {
        // Asegurar que la imagen empiece completamente negra
        Color c = fadeImage.color;
        c.a = 1f;
        fadeImage.color = c;

        // Iniciar transición
        StartCoroutine(FadeToClear());
    }

    private IEnumerator FadeToClear()
    {
        float tiempo = 0f;
        Color c = fadeImage.color;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, tiempo / duracion);
            fadeImage.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }

        // Asegurarse que quede completamente transparente al final
        fadeImage.color = new Color(c.r, c.g, c.b, 0f);
    }
}
