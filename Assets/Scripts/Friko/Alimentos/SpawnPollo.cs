using UnityEngine;
using System.Collections;

public class SpawnPollo : MonoBehaviour
{
    [Header("Objeto a Instanciar")]
    public GameObject prefab;

    [Header("Punto de Spawn")]
    public Transform spawnPoint;

    [Header("Tiempo entre instancias")]
    public float spawnDelay = 2f;

    public void Producir()
    {
        StartCoroutine(SpawnRoutine());
    }

    public void DetenerProduccion()
    {
        StopAllCoroutines();
    }

    IEnumerator SpawnRoutine()
    {
        while (true) // Bucle infinito
        {
            Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
