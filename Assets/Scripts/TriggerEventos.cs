using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;

public class TriggerEventos : MonoBehaviour
{
    [Header("Referencia al barril que rueda")]
    public Animator barrelAnimator;

    [Header("XR Rig o locomotion system a desactivar")]
    public GameObject xrRig;

    [Header("Eventos por falta de botas")]
    public Animator bootsAnimator;
    public string bootsTriggerName = "BootsFail";
    public GameObject objetoDeBotas;

    [Header("Eventos por falta de audífonos")]
    public AudioSource headphonesAudio;
    public AudioSource rayos;
    public AudioSource luces; 

    [Header("UI Fade + Audio de muerte")]
    public RawImage fadeImage;
    public AudioSource grito;

    private void OnTriggerEnter(Collider other)
    {
        ClothesMenu clothes = other.GetComponent<ClothesMenu>();

        if (clothes != null)
        {
            // 🚫 Si falta cualquier prenda → bloquear movimiento XR
            if (!clothes.hasHelmet || !clothes.hasLeftBoot || !clothes.hasRightBoot || !clothes.hasHeadphones)
            {
                SetXRMovement(false);

                // ---- Caso 1: falta el casco → barril ----
                if (!clothes.hasHelmet && barrelAnimator != null)
                {
                    barrelAnimator.SetTrigger("Rodar");

                    BarrelRoll barrel = barrelAnimator.GetComponent<BarrelRoll>();
                    if (barrel != null)
                        barrel.ActivateBarrel();

                    // ❌ El fade + grito lo hace BarrelTrigger al chocar
                }

                // ---- Caso 2: falta alguna bota → animación + objeto + secuencia muerte ----
                if ((!clothes.hasLeftBoot || !clothes.hasRightBoot))
                {
                    if (grito != null && grito.clip != null)
                        grito.Play(); // 🔊 suena de inmediato

                    if (bootsAnimator != null)
                        bootsAnimator.SetTrigger("Rayos");
                        luces.Play();
                        rayos.Play();

                    if (objetoDeBotas != null)
                        objetoDeBotas.SetActive(true);

                    StartCoroutine(FadeAndReload());
                }

                // ---- Caso 3: faltan los audífonos → sonido + secuencia muerte ----
                if (!clothes.hasHeadphones)
                {
                    if (grito != null && grito.clip != null)
                        grito.Play(); // 🔊 suena de inmediato

                    if (headphonesAudio != null)
                        headphonesAudio.Play();

                    StartCoroutine(FadeAndReload());
                }
            }
        }
    }

    private IEnumerator FadeAndReload()
{
    if (fadeImage != null)
    {
        Color c = fadeImage.color;
        c.a = 0f; // Asegurar que inicia transparente
        fadeImage.color = c;

        float duration = 2f; // tiempo del fade (ajústalo a tu gusto)
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, t / duration); // va de 0 → 1
            c.a = alpha;
            fadeImage.color = c;
            yield return null;
        }
    }

    // Espera un poquito antes de reiniciar
    yield return new WaitForSeconds(0.5f);

    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}


    private void SetXRMovement(bool active)
    {
        if (xrRig != null)
            xrRig.SetActive(active);
    }
}
