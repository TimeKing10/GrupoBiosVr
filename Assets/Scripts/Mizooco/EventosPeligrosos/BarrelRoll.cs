using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BarrelRoll : MonoBehaviour
{
    [Header("Velocidad de avance")]
    public float speed = 3f;

    private Rigidbody rb;
    private bool isActive = false;  // 🚫 Al inicio está desactivado

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;   // Física desactivada
        rb.useGravity = false;   // Gravedad desactivada
    }

    void FixedUpdate()
    {
        if (!isActive) return; // ⛔ No hacer nada si no está activo

        Vector3 globalDirection = Vector3.forward;

        rb.linearVelocity = new Vector3(globalDirection.x * speed, rb.linearVelocity.y, globalDirection.z * speed);
    }

    // 👉 Método público para activar el barril
    public void ActivateBarrel()
    {
        isActive = true;
        rb.isKinematic = false;  // Activar física
        rb.useGravity = true;    // Activar gravedad
    }
}
