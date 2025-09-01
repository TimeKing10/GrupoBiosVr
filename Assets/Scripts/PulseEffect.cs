using UnityEngine;
using UnityEngine.UI;

public class PulseEffect : MonoBehaviour
{
    [Header("Configuración")]
    public RawImage targetImage;   // La RawImage a escalar
    public float scaleAmount = 1.2f;  // Tamaño máximo relativo (1.2 = 120%)
    public float speed = 2f;          // Velocidad de la animación

    private Vector3 initialScale;

    void Start()
    {
        if (targetImage == null) targetImage = GetComponent<RawImage>();
        initialScale = targetImage.rectTransform.localScale;
    }

    void Update()
    {
        // Usamos una onda sinusoidal para que suba y baje suavemente
        float scale = 1 + (Mathf.Sin(Time.time * speed) * 0.5f + 0.5f) * (scaleAmount - 1);

        targetImage.rectTransform.localScale = initialScale * scale;
    }
}
