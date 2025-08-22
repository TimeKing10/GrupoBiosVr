using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;

public class MachineUIManager : MonoBehaviour
{
    [Header("UI General")]
    [SerializeField] private GameObject detailPanel; // Panel que se activa al seleccionar

    [Header("Campos individuales")]
    [SerializeField] private TMP_Text idText;
    [SerializeField] private TMP_Text estadoRedText;
    [SerializeField] private TMP_Text estadoMaquinaText;
    [SerializeField] private TMP_Text productoCodigoText;
    [SerializeField] private TMP_Text productoKilosText;
    [SerializeField] private TMP_Text productoDescripcionText;
    [SerializeField] private TMP_Text disponibilidadText;
    [SerializeField] private TMP_Text descripcionTipoParoText;
    [SerializeField] private TMP_Text fechaHoraRecienteParoText;
    [SerializeField] private TMP_Text paquetesMaquinaMinutoText;
    [SerializeField] private TMP_Text paquetesMaquinaSeteadosText;
    [SerializeField] private TMP_Text tonelasPorMaquinaDiaText;

    private void Start()
    {
        if (detailPanel != null)
            detailPanel.SetActive(false); // Al inicio oculto
    }

    /// <summary>
    /// Este método lo llamará cada botón, pasando el índice de la máquina en la lista
    /// </summary>
    public void ShowMachineDetail(int index)
    {
        if (MachineReader.Machines == null || MachineReader.Machines.Count <= index)
        {
            Debug.LogWarning("⚠️ No hay datos de máquina en ese índice");
            return;
        }

        Dictionary<string, object> machine = MachineReader.Machines[index];

        // Mostrar el panel
        if (detailPanel != null) detailPanel.SetActive(true);

        // --- Conversión de estado máquina ---
        string estadoMaquinaStr = "-";
        if (machine.TryGetValue("estadoMaquina", out object estadoObj))
        {
            int estado = -1;
            if (int.TryParse(estadoObj.ToString(), out estado))
            {
                switch (estado)
                {
                    case 0: estadoMaquinaStr = "Apagada"; break;
                    case 1: estadoMaquinaStr = "Encendida"; break;
                    case 2: estadoMaquinaStr = "Con Problemas"; break;
                    default: estadoMaquinaStr = "Desconocido"; break;
                }
            }
        }

        // Rellenar cada campo si existe en el JSON
        idText.text = $"ID: {machine.GetValueOrDefault("idMaquina", "-")}";
        estadoRedText.text = $"Red: {machine.GetValueOrDefault("estadoRed", "-")}";
        estadoMaquinaText.text = $"Estado Máquina: {estadoMaquinaStr}";
        productoCodigoText.text = $"Código Producto: {machine.GetValueOrDefault("productoCodigo", "-")}";
        productoKilosText.text = $"Kilos: {machine.GetValueOrDefault("productoKilos", "-")}";
        productoDescripcionText.text = $"Descripción: {machine.GetValueOrDefault("productoDescripcion", "-")}";
        disponibilidadText.text = $"Disponibilidad: {machine.GetValueOrDefault("disponibilidad", "-")}";
        descripcionTipoParoText.text = $"Tipo Paro: {machine.GetValueOrDefault("descripcionTipoParo", "-")}";
        fechaHoraRecienteParoText.text = $"Último Paro: {machine.GetValueOrDefault("fechaHoraRecienteParo", "-")}";
        paquetesMaquinaMinutoText.text = $"Paquetes/minuto: {machine.GetValueOrDefault("paquetesMaquinaMinuto", "-")}";
        paquetesMaquinaSeteadosText.text = $"Paquetes seteados: {machine.GetValueOrDefault("paquetesMaquinaSeteados", "-")}";
        tonelasPorMaquinaDiaText.text = $"Toneladas/día: {machine.GetValueOrDefault("tonelasPorMaquinaDia", "-")}";
    }

    public void HideDetail()
    {
        if (detailPanel != null)
            detailPanel.SetActive(false);
    }
}

// Helper para evitar errores si falta una clave
public static class DictionaryExtensions
{
    public static object GetValueOrDefault(this Dictionary<string, object> dict, string key, object defaultValue)
    {
        if (dict.ContainsKey(key))
            return dict[key];
        return defaultValue;
    }
}
