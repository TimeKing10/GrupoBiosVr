using UnityEngine;
using System.Collections.Generic;

public class RiskCarousel : MonoBehaviour
{
    [Header("Imagen de ???")]
    public GameObject objetoPregunta;   // La primera imagen con "???"

    [Header("Riesgos con información")]
    public GameObject barrilInfo;
    public GameObject charcoInfo;
    public GameObject montacargasInfo;
    public GameObject maquinaInfo;

    private List<GameObject> imagenes = new List<GameObject>();
    private int indiceActual = 0;

    void Start()
    {
        if (GameStateManager.Instance == null) return;

        ConstruirLista();   // <- Siempre arma la lista al iniciar escena
        MostrarImagen(0);   // Mostrar la primera
    }

    void ConstruirLista()
    {
        imagenes.Clear(); // Limpia la lista

        // Agregar ??? por defecto
        imagenes.Add(objetoPregunta);

        // Revisar qué riesgos ya están detectados
        if (GameStateManager.Instance.barrilDetectado)
            imagenes.Add(barrilInfo);

        if (GameStateManager.Instance.charcoDetectado)
            imagenes.Add(charcoInfo);

        if (GameStateManager.Instance.montacargasDetectado)
            imagenes.Add(montacargasInfo);

        if (GameStateManager.Instance.maquinaDetectada)
            imagenes.Add(maquinaInfo);

        // Si ya están todos detectados, quitamos el ???
        bool todosDetectados =
            GameStateManager.Instance.barrilDetectado &&
            GameStateManager.Instance.charcoDetectado &&
            GameStateManager.Instance.montacargasDetectado &&
            GameStateManager.Instance.maquinaDetectada;

        if (todosDetectados)
        {
            imagenes.Remove(objetoPregunta);
            objetoPregunta.SetActive(false);
        }
        else
        {
            objetoPregunta.SetActive(true);
        }

        indiceActual = 0; // Reinicia el índice
    }

    void MostrarImagen(int index)
    {
        // Apagar todas
        objetoPregunta.SetActive(false);
        barrilInfo.SetActive(false);
        charcoInfo.SetActive(false);
        montacargasInfo.SetActive(false);
        maquinaInfo.SetActive(false);

        // Encender solo la actual
        if (imagenes.Count > 0 && index >= 0 && index < imagenes.Count)
        {
            imagenes[index].SetActive(true);
        }
    }

    public void Siguiente()
    {
        if (imagenes.Count == 0) return;
        indiceActual = (indiceActual + 1) % imagenes.Count;
        MostrarImagen(indiceActual);
    }

    public void Anterior()
    {
        if (imagenes.Count == 0) return;
        indiceActual = (indiceActual - 1 + imagenes.Count) % imagenes.Count;
        MostrarImagen(indiceActual);
    }
}
