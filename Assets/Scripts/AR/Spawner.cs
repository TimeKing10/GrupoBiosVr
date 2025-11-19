using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Prefab a instanciar")]
    public GameObject prefab;

    [Header("Punto de spawn (puede ser un Empty en la escena)")]
    public Transform spawnPoint;

    // Método público para usar con un botón
    public void SpawnObject()
    {
        if (prefab != null && spawnPoint != null)
        {
            Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
            Debug.Log("Objeto instanciado desde botón.");
        }
        else
        {
            Debug.LogWarning("Falta asignar el prefab o el spawn point en el inspector.");
        }
    }
}
