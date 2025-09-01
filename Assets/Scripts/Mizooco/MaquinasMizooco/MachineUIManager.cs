using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;

public class MachineUIManager : MonoBehaviour
{
    [Header("UI General")]
    [SerializeField] private GameObject detailPanel; 

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

    private int currentIndex = 0; // 👉 índice actual

    private void Start()
    {
        if (detailPanel != null) detailPanel.SetActive(false);
    }

    public void ShowMachineDetail(int index)
    {
        if (MachineReader.Machines == null || MachineReader.Machines.Count <= index || index < 0)
        {
            Debug.LogWarning("⚠️ No hay datos de máquina en ese índice");
            return;
        }

        currentIndex = index; // guardar el índice actual

        Dictionary<string, object> machine = MachineReader.Machines[index];

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

        // Rellenar cada campo
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

    // 👉 Métodos para botones
    public void NextMachine()
    {
        if (MachineReader.Machines == null || MachineReader.Machines.Count == 0) return;

        currentIndex++;
        if (currentIndex >= MachineReader.Machines.Count)
            currentIndex = 0; // volver al inicio

        ShowMachineDetail(currentIndex);
    }

    public void PreviousMachine()
    {
        if (MachineReader.Machines == null || MachineReader.Machines.Count == 0) return;

        currentIndex--;
        if (currentIndex < 0)
            currentIndex = MachineReader.Machines.Count - 1; // ir al último

        ShowMachineDetail(currentIndex);
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
