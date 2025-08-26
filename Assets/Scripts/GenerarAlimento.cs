using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Playables;

public class GenerarAlimento : MonoBehaviour
{
    [Header("Identificación")]
    public string machineId; // asignar por inspector (ej: "PQ0")

    [Header("Spawn Bolsa")]
    public GameObject[] prefabs;
    public Transform puntoSpawn;

    [Header("Timeline")]
    public PlayableDirector director;

    [Header("Opciones")]
    public int prefabSeleccionado = 0;

    private GameObject bolsaActual;
    private Dictionary<string, int> lookupByName;

    void Awake()
    {
        BuildLookup();
    }

    void Start()
    {
        var mm = FindObjectOfType<MachineManager>();
        if (mm != null)
            mm.RegisterGenerador(this);

        if (director != null)
        {
            director.stopped += OnTimelineEnd;
            director.Play();
        }
    }

    void BuildLookup()
    {
        lookupByName = new Dictionary<string, int>();
        for (int i = 0; i < prefabs.Length; i++)
        {
            if (prefabs[i] == null) continue;
            string key = Normalize(prefabs[i].name);
            if (!lookupByName.ContainsKey(key))
                lookupByName.Add(key, i);
        }
    }

    string Normalize(string s)
    {
        if (string.IsNullOrEmpty(s)) return "";
        s = s.ToUpperInvariant().Trim();
        s = s.Replace("+", " "); // elimina o convierte + en espacio para normalizar
        s = Regex.Replace(s, @"[^\w\s]", ""); // quitar otros signos de puntuación
        s = Regex.Replace(s, @"\s+", " ");    // colapsar espacios
        return s.Trim();
    }

    string ExtractBaseName(string productDescription)
    {
        if (string.IsNullOrEmpty(productDescription)) return "";

        string norm = Normalize(productDescription);
        var tokens = new List<string>(norm.Split(' '));

        while (tokens.Count > 0)
        {
            string last = tokens[tokens.Count - 1];
            if (Regex.IsMatch(last, @"^\d+(\.\d+)?K$") ||
                Regex.IsMatch(last, @"^\d+(\.\d+)?KG$") ||
                Regex.IsMatch(last, @"^\d+(\.\d+)?$") ||
                Regex.IsMatch(last, @"^[\d,]+$"))
            {
                tokens.RemoveAt(tokens.Count - 1);
                continue;
            }
            if (last.EndsWith("K") && Regex.IsMatch(last.Substring(0, last.Length - 1), @"^\d+(\.\d+)?$"))
            {
                tokens.RemoveAt(tokens.Count - 1);
                continue;
            }
            break;
        }

        return string.Join(" ", tokens).Trim();
    }

    public void SeleccionarAlimento1()
    {
        SeleccionarPrefab(0);
    }

    public void SeleccionarAlimento2()
    {
        SeleccionarPrefab(1);
    }

    private void SeleccionarPrefab(int indice)
    {
        if (indice >= 0 && indice < prefabs.Length)
        {
            prefabSeleccionado = indice;
            Debug.Log($"[GenerarAlimento:{gameObject.name}] Prefab seleccionado manualmente: {prefabs[indice].name}");
        }
        else
        {
            Debug.LogWarning($"[GenerarAlimento:{gameObject.name}] Índice de prefab fuera de rango.");
        }
    }

    public void SeleccionarPorDescripcion(string productoDescripcion)
    {
        string baseName = ExtractBaseName(productoDescripcion);
        if (string.IsNullOrEmpty(baseName))
        {
            Debug.LogWarning($"[GenerarAlimento:{gameObject.name}] Descripción vacía al intentar seleccionar prefab.");
            return;
        }

        string key = Normalize(baseName);

        if (lookupByName.TryGetValue(key, out int idx))
        {
            prefabSeleccionado = idx;
            Debug.Log($"[GenerarAlimento:{gameObject.name}] Seleccionado automáticamente prefab '{prefabs[idx].name}' para '{productoDescripcion}' (base='{baseName}').");
            return;
        }

        foreach (var kv in lookupByName)
        {
            if (key.Contains(kv.Key) || kv.Key.Contains(key))
            {
                prefabSeleccionado = kv.Value;
                Debug.Log($"[GenerarAlimento:{gameObject.name}] Fallback: seleccionado '{prefabs[kv.Value].name}' para '{productoDescripcion}'.");
                return;
            }
        }

        Debug.LogWarning($"[GenerarAlimento:{gameObject.name}] No se encontró prefab para '{productoDescripcion}' (base='{baseName}').");
    }

    public void CrearBolsa()
    {
        if (bolsaActual != null)
        {
            Debug.Log($"[GenerarAlimento:{gameObject.name}] Ya hay una bolsa en el gancho, no se crea otra.");
            return;
        }

        Collider[] objetosEnSpawn = Physics.OverlapSphere(puntoSpawn.position, 0.2f);
        foreach (var obj in objetosEnSpawn)
        {
            if (obj.CompareTag("Bolsa"))
            {
                bolsaActual = obj.gameObject;
                bolsaActual.transform.SetParent(puntoSpawn);
                if (bolsaActual.TryGetComponent<Rigidbody>(out Rigidbody rb)) rb.isKinematic = true;
                Debug.Log($"[GenerarAlimento:{gameObject.name}] Reutilizando bolsa existente: {bolsaActual.name}");
                return;
            }
        }

        if (prefabSeleccionado < 0 || prefabSeleccionado >= prefabs.Length)
        {
            Debug.LogWarning($"[GenerarAlimento:{gameObject.name}] Prefab seleccionado inválido.");
            return;
        }

        bolsaActual = Instantiate(prefabs[prefabSeleccionado], puntoSpawn.position, puntoSpawn.rotation);
        bolsaActual.transform.SetParent(puntoSpawn);
        if (bolsaActual.TryGetComponent<Rigidbody>(out Rigidbody rbNew))
            rbNew.isKinematic = true;

        Debug.Log($"[GenerarAlimento:{gameObject.name}] Creada bolsa: {prefabs[prefabSeleccionado].name}");
    }

    public void SoltarBolsa()
    {
        if (bolsaActual != null && bolsaActual.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.isKinematic = false;
            bolsaActual.transform.parent = null;
            Debug.Log($"[GenerarAlimento:{gameObject.name}] Soltada bolsa: {bolsaActual.name}");
            bolsaActual = null;
        }
    }

    private void OnTimelineEnd(PlayableDirector d)
    {
        d.time = 0;
        d.Play();
    }
}
