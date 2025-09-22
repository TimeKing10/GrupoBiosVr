using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public class MachineSinglePanel : MonoBehaviour
{
    [Header("Identificador de la máquina (poner en el prefab)")]
    [Tooltip("El valor que coincide con la clave 'idMaquina' dentro del JSON")]
    public string machineId;

    [Header("Referencias UI")]
    [SerializeField] private TMP_Text idText;
    [SerializeField] private TMP_Text estadoText;

    [Header("Opciones")]
    [Tooltip("Texto mostrado cuando no se encuentra la máquina")]
    public string notFoundText = "N/A";

    [Tooltip("Intervalo en segundos para comprobar si llegaron nuevos datos")]
    public float pollInterval = 0.5f;

    private Coroutine pollingCoroutine;

    private void OnEnable()
    {
        // Iniciar polling para detectar cambios en MachineReader.Machines
        pollingCoroutine = StartCoroutine(PollMachinesRoutine());
        // Intentar una actualización inmediata
        Refresh();
    }

    private void OnDisable()
    {
        if (pollingCoroutine != null)
        {
            StopCoroutine(pollingCoroutine);
            pollingCoroutine = null;
        }
    }

    private IEnumerator PollMachinesRoutine()
    {
        while (true)
        {
            Refresh();
            yield return new WaitForSeconds(pollInterval);
        }
    }

    /// <summary>
    /// Forzar actualización (puede llamarse desde otros scripts / eventos).
    /// </summary>
    public void Refresh()
    {
        var machines = MachineReader.Machines;

        if (machines == null || machines.Count == 0)
        {
            ShowNotFound();
            return;
        }

        if (string.IsNullOrEmpty(machineId))
        {
            ShowNotFound();
            return;
        }

        // Buscar la primera coincidencia por "idMaquina"
        Dictionary<string, object> found = null;

        for (int i = 0; i < machines.Count; i++)
        {
            var dict = machines[i];
            if (dict == null) continue;

            if (dict.ContainsKey("idMaquina"))
            {
                var val = dict["idMaquina"];
                if (val != null && val.ToString() == machineId)
                {
                    found = dict;
                    break;
                }
            }
        }

        if (found != null)
            ShowFromDictionary(found);
        else
            ShowNotFound();
    }

    /// <summary>
    /// Cambiar el id de la máquina en tiempo de ejecución y refrescar.
    /// </summary>
    public void SetMachineId(string newId)
    {
        machineId = newId;
        Refresh();
    }

    private void ShowFromDictionary(Dictionary<string, object> dict)
    {
        string idVal = GetStringFromDict(dict, "idMaquina");
        string estadoVal = GetStringFromDict(dict, "estadoMaquina");

        if (idText != null) idText.text = string.IsNullOrEmpty(idVal) ? notFoundText : idVal;
        if (estadoText != null) estadoText.text = string.IsNullOrEmpty(estadoVal) ? notFoundText : estadoVal;
    }

    private string GetStringFromDict(Dictionary<string, object> dict, string key)
    {
        if (dict == null || !dict.ContainsKey(key) || dict[key] == null)
            return null;

        // Si el valor es un JArray u otro tipo, ToString() lo representará
        return dict[key].ToString();
    }

    private void ShowNotFound()
    {
        if (idText != null) idText.text = notFoundText;
        if (estadoText != null) estadoText.text = notFoundText;
    }
}
