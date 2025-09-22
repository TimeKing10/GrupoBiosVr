using UnityEngine;
using TMPro;
using System.Collections;

public class Popup : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI popupText; 
    public CanvasGroup canvasGroup;   // Controla el fade in/out
    public float fadeDuration = 0.5f; // Tiempo de aparición/desaparición
    public float showTime = 2f;       // Tiempo que permanece visible

    [Header("Audio")]
    public AudioSource audioSource;   // Con el clip ya asignado en el inspector

    private Coroutine currentRoutine;

    void Start()
    {
        if (canvasGroup != null)
            canvasGroup.alpha = 0; // Arranca invisible
    }

    public void Show()
{
    if (canvasGroup == null) return;

    // Detener rutina previa si había un popup en curso
    if (currentRoutine != null)
        StopCoroutine(currentRoutine);

    // Reproducir sonido (si está configurado)
    if (audioSource != null)
        audioSource.Play();

    currentRoutine = StartCoroutine(ShowAndFadeRoutine());
}

    private IEnumerator ShowAndFadeRoutine()
    {
        // 🔹 Fade In
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, elapsed / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1;

        // 🔹 Mantener visible
        yield return new WaitForSeconds(showTime);

        // 🔹 Fade Out
        elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, elapsed / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0;
    }
}
