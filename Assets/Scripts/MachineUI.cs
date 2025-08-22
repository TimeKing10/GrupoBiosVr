using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MachineUI : MonoBehaviour
{
    [Header("ID de máquina que representa este panel")]
    public int machineIndex;

    [Header("Referencias de UI")]
    public TMP_Text idText;
    public TMP_Text estadoText;
    public TMP_Text productoText;
    public TMP_Text prodPorMinutoText;
    public TMP_Text cantProducidoText;
    public RawImage estadoColor;

    private float updateTimer = 0f;
    private float updateInterval = 5f;
    private bool datosMostrados = false; // Para saber si ya se mostró algo al menos una vez

    void Update()
    {
        // Si no hay datos todavía, intentar actualizarlos y no contar tiempo
        if (MachineReader.Machines == null || MachineReader.Machines.Count <= machineIndex)
        {
            UpdateUI();
            return;
        }

        // Si los datos se acaban de mostrar por primera vez, reiniciar el contador
        if (!datosMostrados)
        {
            UpdateUI();
            datosMostrados = true;
            updateTimer = 0f;
            return;
        }

        // Contar tiempo normalmente
        updateTimer += Time.deltaTime;
        if (updateTimer >= updateInterval)
        {
            UpdateUI();
            updateTimer = 0f;
        }
    }

    void UpdateUI()
    {
        if (MachineReader.Machines == null || MachineReader.Machines.Count <= machineIndex)
            return;

        Dictionary<string, object> machine = MachineReader.Machines[machineIndex];

        idText.text = "ID: " + machine["idMaquina"].ToString();
        productoText.text = "Producto: " + machine["productoDescripcion"].ToString();

        if (float.TryParse(machine["paquetesMaquinaMinuto"].ToString(), out float ppm))
            prodPorMinutoText.text = "Prod/min: " + ppm.ToString("F2");
        else
            prodPorMinutoText.text = "Prod/min: 0.00";

        // Cambié por paquetesMaquinaSeteados en vez de producción diaria
        if (float.TryParse(machine["paquetesMaquinaSeteados"].ToString(), out float paquetes))
            cantProducidoText.text = "Paquetes seteados: " + paquetes.ToString("F0");
        else
            cantProducidoText.text = "Paquetes seteados: 0";

        if (int.TryParse(machine["estadoMaquina"].ToString(), out int estado))
        {
            estadoText.text = "Estado: " + (estado == 1 ? "Encendida" : "Apagada");
            estadoColor.color = estado == 1 ? Color.green : Color.red;
        }
    }
}
