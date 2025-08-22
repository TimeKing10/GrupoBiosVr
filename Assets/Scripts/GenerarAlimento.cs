using UnityEngine;
using UnityEngine.Playables;

public class GenerarAlimento : MonoBehaviour
{
    [Header("Spawn Bolsa")]
    public GameObject[] prefabs; // Lista de opciones de prefabs
    public Transform puntoSpawn;

    [Header("Timeline")]
    public PlayableDirector director;

    private GameObject bolsaActual;
    private int prefabSeleccionado = 0;

    void Start()
    {
        // Iniciar el ciclo
        if (director != null)
        {
            director.stopped += OnTimelineEnd;
            director.Play();
        }
    }

    // 📌 Métodos para los botones en el OnClick
    public void SeleccionarAlimento1()
    {
        SeleccionarPrefab(0);
    }

    public void SeleccionarAlimento2()
    {
        SeleccionarPrefab(1);
    }

    // 📌 Método interno para cambiar el prefab
    private void SeleccionarPrefab(int indice)
    {
        if (indice >= 0 && indice < prefabs.Length)
        {
            prefabSeleccionado = indice;
            Debug.Log($"✅ Prefab seleccionado: {prefabs[indice].name}");
        }
        else
        {
            Debug.LogWarning("⚠️ Índice de prefab fuera de rango.");
        }
    }

    // 📌 Llamar este método desde un evento en la Timeline
    public void CrearBolsa()
    {
        if (bolsaActual != null) 
        {
            Debug.Log("⚠️ Ya hay una bolsa en el gancho, no se crea otra.");
            return;
        }

        Collider[] objetosEnSpawn = Physics.OverlapSphere(puntoSpawn.position, 0.2f);
        foreach (var obj in objetosEnSpawn)
        {
            if (obj.CompareTag("Bolsa"))
            {
                Debug.Log("⚠️ Hay una bolsa residual, se reutiliza como bolsaActual.");
                bolsaActual = obj.gameObject;
                bolsaActual.transform.SetParent(puntoSpawn);
                return;
            }
        }

        bolsaActual = Instantiate(prefabs[prefabSeleccionado], puntoSpawn.position, puntoSpawn.rotation);
        bolsaActual.transform.SetParent(puntoSpawn);

        if (bolsaActual.TryGetComponent<Rigidbody>(out Rigidbody rb))
            rb.isKinematic = true;
    }

    public void SoltarBolsa()
    {
        if (bolsaActual != null && bolsaActual.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.isKinematic = false;
            bolsaActual.transform.parent = null;
            bolsaActual = null;
        }
    }

    private void OnTimelineEnd(PlayableDirector d)
    {
        d.time = 0;
        d.Play();
    }
}
