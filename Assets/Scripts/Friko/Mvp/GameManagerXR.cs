using UnityEngine;
using TMPro;

public class GameManagerXR : MonoBehaviour
{
    [Header("Contador de Paquetes")]
    public int paquetesEntregados = 0;
    public int metaPaquetes = 10;

    [Header("UI - Paquetes")]
    public TextMeshProUGUI textoPaquetes;

    [Header("Timer")]
    public float tiempoInicial = 60f;  
    private float tiempoRestante;
    private bool contando = false;   // ❗ Ahora NO empieza contando

    [Header("UI - Timer")]
    public TextMeshProUGUI textoTiempo;

    [Header("Acciones al finalizar")]
    public GameObject locomotion;  

    public GameObject fondo;      
    public GameObject panelExito;        
    public GameObject panelFracaso;      
    public AudioSource audioExito;       
    public AudioSource audioFracaso;

    [Header("UI Inicio")]
    public GameObject panelInicio;   // <- El panel que muestra la introducción

    private void Start()
    {
        tiempoRestante = tiempoInicial;

        // Mostrar UI inicial
        if (panelInicio != null) panelInicio.SetActive(true);

        // El juego todavía no arranca
        ActualizarUI();
        ActualizarTimer();
    }

    private void Update()
    {
        if (!contando) return;

        tiempoRestante -= Time.deltaTime;

        if (tiempoRestante <= 0)
        {
            tiempoRestante = 0;
            contando = false;
            ActualizarTimer();
            FinDelTiempo();
            return;
        }

        ActualizarTimer();
    }

    // ----------------------------------------------------------
    // 🔵 MÉTODO QUE SE LLAMA AL OPRIMIR UN BOTÓN
    // ----------------------------------------------------------
    public void StartGame()
    {
        Debug.Log("Juego iniciado!");

        contando = true;
        tiempoRestante = tiempoInicial;
        
        // Ocultar panel de introducción
        if (panelInicio != null) panelInicio.SetActive(false);
    }

    // ---------------------------
    //  MÉTODOS DE PUNTOS
    // ---------------------------
    public void SumarPaquete()
    {
        if (paquetesEntregados < metaPaquetes)
        {
            paquetesEntregados++;
            ActualizarUI();
        }
    }

    public void RestarPaquete()
    {
        if (paquetesEntregados > 0)
        {
            paquetesEntregados--;
            ActualizarUI();
        }
    }

    private void ActualizarUI()
    {
        if (textoPaquetes != null)
            textoPaquetes.text = paquetesEntregados + "/" + metaPaquetes;
    }

    // ---------------------------
    //  MÉTODOS DEL TIMER
    // ---------------------------
    private void ActualizarTimer()
    {
        if (textoTiempo == null) return;

        int minutos = Mathf.FloorToInt(tiempoRestante / 60f);
        int segundos = Mathf.FloorToInt(tiempoRestante % 60f);

        textoTiempo.text = $"{minutos:0}:{segundos:00}";
    }

    private void FinDelTiempo()
    {
        Debug.Log("⏱ Se acabó el tiempo!");
        if (fondo != null) fondo.SetActive(true);

        if (locomotion != null)
            locomotion.SetActive(false);

        if (panelExito != null) panelExito.SetActive(true);    

        if (paquetesEntregados >= metaPaquetes)
        {
            if (panelExito != null) panelExito.SetActive(true);
            if (audioExito != null) audioExito.Play();
            
        }
        else
        {
            if (panelFracaso != null) panelFracaso.SetActive(true);
            if (audioFracaso != null) audioFracaso.Play();
            
        }
    }
}
