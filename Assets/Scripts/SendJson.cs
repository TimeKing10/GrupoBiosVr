using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class SendJsonFromList : MonoBehaviour
{
    [Header("Configuración API")]
    public string apiUrl = "https://iis.automation4ir.com/Demo_GD/api/mizooco/empaque";

    [Header("Índice de la máquina en la lista (de 0 a N)")]
    public int indiceMaquina = 0;

    private bool on = false;
    private float ultimaVezPresionado = 0f;
    private float tiempoBloqueo = 0.3f;

    [System.Serializable]
    public class MaquinaData
    {
        public string id_maquina;
        public bool paro_forzado;
    }

    // Método para prender (paro_forzado = false)
    public void PrenderMaquina()
    {
        if (Time.time - ultimaVezPresionado < tiempoBloqueo) return;

        if (!on)
        {
            EnviarEstadoDesdeIndice(indiceMaquina, false);
            on = true;
            ultimaVezPresionado = Time.time; // 🔹 Actualiza tiempo
        }
    }

    public void ApagarMaquina()
    {
        if (Time.time - ultimaVezPresionado < tiempoBloqueo) return;

        if (on)
        {
            EnviarEstadoDesdeIndice(indiceMaquina, true);
            on = false;
            ultimaVezPresionado = Time.time; // 🔹 Actualiza tiempo
            Debug.Log("Papi se apago");
        }
}

    void EnviarEstadoDesdeIndice(int indice, bool paro)
    {
        // Asumimos que MachineReader.Machines es List<Dictionary<string, object>>
        if (MachineReader.Machines != null && indice >= 0 && indice < MachineReader.Machines.Count)
        {
            var maquinaDic = MachineReader.Machines[indice];
            if (maquinaDic.TryGetValue("idMaquina", out object idObj))
            {
                string idMaquinaSeleccionado = idObj.ToString();
                Debug.Log($"Enviando estado de máquina '{idMaquinaSeleccionado}' con paro_forzado={paro}");
                EnviarEstado(idMaquinaSeleccionado, paro);
            }
            else
            {
                Debug.LogError("La máquina no tiene la clave 'idMaquina'.");
            }
        }
        else
        {
            Debug.LogError("Índice de máquina inválido o lista no cargada.");
        }
    }

    void EnviarEstado(string idMaquina, bool paro)
    {
        MaquinaData data = new MaquinaData
        {
            id_maquina = idMaquina,
            paro_forzado = paro
        };

        string json = JsonUtility.ToJson(data);
        Debug.Log("📦 JSON generado:\n" + json);

        StartCoroutine(PostRequest(apiUrl, json));
    }

    IEnumerator PostRequest(string url, string json)
    {
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("✅ Datos enviados correctamente: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("❌ Error al enviar datos: " + request.error);
        }
    }
}
