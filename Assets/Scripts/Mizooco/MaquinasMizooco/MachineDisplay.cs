using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MachineDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text infoText;
    [SerializeField] private int machineIndex = 0;

    private void Start()
    {
        StartCoroutine(WaitAndDisplay());
    }

    private System.Collections.IEnumerator WaitAndDisplay()
    {
        // Espera hasta que MachineReader tenga los datos listos
        while (MachineReader.Machines == null || MachineReader.Machines.Count == 0)
            yield return null;

        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (machineIndex < 0 || machineIndex >= MachineReader.Machines.Count)
        {
            Debug.LogError($"❌ Índice {machineIndex} fuera de rango para {gameObject.name}");
            return;
        }

        Dictionary<string, object> machine = MachineReader.Machines[machineIndex];
        string info = "";

        foreach (var kvp in machine)
        {
            if (kvp.Value is Newtonsoft.Json.Linq.JArray array)
            {
                List<string> values = new List<string>();
                foreach (var item in array)
                    values.Add(item.ToString());

                info += $"{kvp.Key}: [{string.Join(", ", values)}]\n";
            }
            else
            {
                info += $"{kvp.Key}: {kvp.Value}\n";
            }
        }

        infoText.text = info;
    }
}
