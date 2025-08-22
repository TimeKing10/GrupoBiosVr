using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VRSlowTeleport : MonoBehaviour
{
    [Header("Referencia al XR Rig")]
    public Transform xrRig;

    [Header("Fade en RawImage")]
    public RawImage fadeImage;

    [Header("Tiempos de Fade")]
    public float fadeDuration = 1.5f;   // Tiempo en segundos para oscurecer/aclarar
    public float blackHoldTime = 0.3f;  // Tiempo en negro antes de aclarar

    private bool isTeleporting = false;
    private bool canTeleport = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canTeleport = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canTeleport = false;
        }
    }

    public void TeleportTo(Transform destino)
    {
        if (canTeleport && !isTeleporting)
        {
            StartCoroutine(TeleportRoutine(destino));
        }
    }

    IEnumerator TeleportRoutine(Transform destino)
    {
        isTeleporting = true;

        // Fade Out
        yield return StartCoroutine(Fade(0f, 1f, fadeDuration));

        // Calcula offset para que no cambie la altura ni orientación relativa
        Vector3 offset = xrRig.position - Camera.main.transform.position;
        offset.y = 0;
        xrRig.position = destino.position + offset;

        // Mantener en negro un momento
        yield return new WaitForSeconds(blackHoldTime);

        // Fade In
        yield return StartCoroutine(Fade(1f, 0f, fadeDuration));

        isTeleporting = false;
    }

    IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        Color c = fadeImage.color;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            c.a = Mathf.Lerp(startAlpha, endAlpha, t);
            fadeImage.color = c;
            yield return null;
        }
    }
}
