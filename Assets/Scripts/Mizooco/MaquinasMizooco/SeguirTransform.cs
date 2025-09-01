using UnityEngine;

public class SeguirTransform : MonoBehaviour
{
    [Header("Transform objetivo (el que se va a seguir)")]
    public Transform objetivo;

    [Header("Velocidad de movimiento")]
    public float velocidad = 5f;

    void FixedUpdate()
    {
        if (objetivo == null) return;

        // Moverse hacia el objetivo de forma suave
        transform.position = Vector3.MoveTowards(
            transform.position,
            objetivo.position,
            velocidad * Time.deltaTime
        );
    }
}
