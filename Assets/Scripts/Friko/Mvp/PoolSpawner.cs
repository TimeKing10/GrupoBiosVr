using UnityEngine;
using System.Collections;

public class PoolSpawner : MonoBehaviour
{
    [Header("Pool")]
    public GameObject[] objetosPool;
    public Transform puntoInicio;

    [Header("Tiempo")]
    public float tiempoEntreSpawns = 1f;

    private int indiceActual = 0;

    private void Start()
    {
        // Mandar todos al inicio y apagarlos
        foreach (GameObject obj in objetosPool)
        {
            obj.transform.position = puntoInicio.position;
            obj.SetActive(false);
        }

        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            GameObject obj = objetosPool[indiceActual];

            obj.transform.position = puntoInicio.position;
            obj.SetActive(true);

            indiceActual++;
            if (indiceActual >= objetosPool.Length)
                indiceActual = 0;

            yield return new WaitForSeconds(tiempoEntreSpawns);
        }
    }
}