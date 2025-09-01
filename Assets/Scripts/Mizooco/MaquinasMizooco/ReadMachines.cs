using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class MachineReader : MonoBehaviour
{
    public static List<Dictionary<string, object>> Machines { get; private set; }

    [SerializeField] private string apiURL = "https://iis.automation4ir.com/Demo_GD/api/mizooco/empaque";
    [SerializeField] private float refreshInterval = 20f; // tiempo en segundos

    void Awake()
    {
        StartCoroutine(UpdateMachinesLoop());
    }

    private IEnumerator UpdateMachinesLoop()
    {
        while (true) // Bucle infinito para seguir actualizando
        {
            yield return LoadMachinesFromAPI();
            yield return new WaitForSeconds(refreshInterval);
        }
    }

    private IEnumerator LoadMachinesFromAPI()
    {
        UnityWebRequest request = UnityWebRequest.Get(apiURL);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("❌ Error al obtener datos: " + request.error);
            yield break;
        }

        string jsonText = request.downloadHandler.text;

        try
        {
            Machines = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jsonText);

            Debug.Log("✅ Datos obtenidos de la API:");
            Debug.Log(JsonConvert.SerializeObject(Machines, Formatting.Indented));
        }
        catch (System.Exception ex)
        {
            Debug.LogError("❌ Error al deserializar el JSON de la API: " + ex.Message);
        }
    }
}
