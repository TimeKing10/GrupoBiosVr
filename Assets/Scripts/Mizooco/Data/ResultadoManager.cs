using UnityEngine;
using TMPro;

public class ResultadoManager : MonoBehaviour
{
    [Header("Caritas")]
    public GameObject caritaTriste;
    public GameObject caritaNormal;
    public GameObject caritaFeliz;

    [Header("UI")]
    public TMP_Text textoResultado;

    void Start()
    {
        int valor = ContadorManager.Instance.GetContador();
        Debug.Log("El contador quedó en: " + valor);

        // 🔹 Mostrar el número en el TextMeshPro
        if (textoResultado != null)
        {
            textoResultado.text = valor.ToString();
        }

        // 🔹 Apagar todas las caritas
        caritaTriste.SetActive(false);
        caritaNormal.SetActive(false);
        caritaFeliz.SetActive(false);

        // 🔹 Encender la que corresponda según el valor
        if (valor == 0 || valor == 1)
        {
            caritaTriste.SetActive(true);
        }
        else if (valor == 2 || valor == 3)
        {
            caritaNormal.SetActive(true);
        }
        else if (valor == 4)
        {
            caritaFeliz.SetActive(true);
        }
    }
}
