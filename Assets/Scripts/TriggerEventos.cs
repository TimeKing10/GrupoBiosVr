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

    [Header("Escena de Game Over / Muerte")]
    public string nextSceneName = "GameOverScene"; // 👉 cámbialo en el Inspector

    private void OnTriggerEnter(Collider other)
    {
        ClothesMenu clothes = other.GetComponent<ClothesMenu>();

        if (clothes != null)
        {
            if (!clothes.hasHelmet || !clothes.hasLeftBoot || !clothes.hasRightBoot || !clothes.hasHeadphones)
            {
                SetXRMovement(false);

                if (!clothes.hasHelmet && barrelAnimator != null)
                {
                    barrelAnimator.SetTrigger("Rodar");

                    BarrelRoll barrel = barrelAnimator.GetComponent<BarrelRoll>();
                    if (barrel != null)
                        barrel.ActivateBarrel();
                }

                if ((!clothes.hasLeftBoot || !clothes.hasRightBoot))
                {
                    if (grito != null && grito.clip != null)
                        grito.Play();

                    if (bootsAnimator != null)
                        bootsAnimator.SetTrigger("Rayos");

                    if (luces != null) luces.Play();
                    if (rayos != null) rayos.Play();

                    if (objetoDeBotas != null)
                        objetoDeBotas.SetActive(true);

                    StartCoroutine(FadeAndLoadScene());
                }

                if (!clothes.hasHeadphones)
                {
                    if (grito != null && grito.clip != null)
                        grito.Play();

                    if (headphonesAudio != null)
                        headphonesAudio.Play();

                    StartCoroutine(FadeAndLoadScene());
                }
            }
        }
    }

    private IEnumerator FadeAndLoadScene()
    {
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;

            float duration = 2f;
            float t = 0f;

            while (t < duration)
            {
                t += Time.deltaTime;
                float alpha = Mathf.Lerp(0f, 1f, t / duration);
                c.a = alpha;
                fadeImage.color = c;
                yield return null;
            }
        }

        yield return new WaitForSeconds(0.5f);

        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
        else
            Debug.LogWarning("⚠️ No se asignó nombre de escena en TriggerEventos");
    }

    private void SetXRMovement(bool active)
    {
        if (xrRig != null)
            xrRig.SetActive(active);
    }
}
