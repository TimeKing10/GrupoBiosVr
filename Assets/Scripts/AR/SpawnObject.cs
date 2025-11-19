using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    [Header("Prefab a clonar")]
    public GameObject prefab;

    [Header("Lugar donde aparece")]
    public Transform puntoSpawn;

    public void GenerarObjeto()
    {
        Instantiate(prefab, puntoSpawn.position, puntoSpawn.rotation);
    }
}
