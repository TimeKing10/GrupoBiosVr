using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class ContadorPaquetes : MonoBehaviour
{
    [Header("Conteo")]
    public int cantidad = 0;
    public int maximo = 7;

    [Header("Paquetes visuales (ordenados)")]
    public GameObject[] paquetesVisuales;

    [Header("Zona para mandar paquetes reales (Alimento)")]
    public Transform zonaDescarte;

    [Header("UI")]
    public TextMeshProUGUI textoContador;

    private HashSet<ContadorPadre> paquetesContados = new HashSet<ContadorPadre>();
    private int indiceVisual = 0;

    private void Start()
    {
        // Apagar visuales
        foreach (var p in paquetesVisuales)
        {
            if (p != null)
                p.SetActive(false);
        }

        ActualizarUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 🔹 SOLO objetos Alimento
        if (!other.CompareTag("Alimento"))
            return;

        ContadorPadre contador = other.GetComponent<ContadorPadre>();
        if (contador == null)
            return;

        // 🟢 VISUAL SIEMPRE
        ActivarPaqueteVisual();

        // ➕ CONTEO SOLO SI ES VÁLIDO
        if (contador.contador == 3 && !paquetesContados.Contains(contador))
        {
            paquetesContados.Add(contador);

            if (cantidad < maximo)
                cantidad++;

            ActualizarUI();
        }

        // 🚀 SIEMPRE tepear el alimento real
        TeletransportarAlimento(other);
    }

    private void ActivarPaqueteVisual()
    {
        if (indiceVisual >= paquetesVisuales.Length)
            return;

        paquetesVisuales[indiceVisual].SetActive(true);
        indiceVisual++;
    }

    private void TeletransportarAlimento(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        other.transform.position = zonaDescarte.position;
        other.transform.rotation = zonaDescarte.rotation;
    }

    private void ActualizarUI()
    {
        if (textoContador != null)
            textoContador.text = cantidad + " / " + maximo;
    }

    public void ResetearCanastaVisual()
    {
        cantidad = 0;
        indiceVisual = 0;

        foreach (var p in paquetesVisuales)
        {
            if (p != null)
                p.SetActive(false);
        }

        paquetesContados.Clear();
        ActualizarUI();
    }
}
