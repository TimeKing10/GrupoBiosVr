using UnityEngine;

public class AnimatorTrigger : MonoBehaviour
{
    [SerializeField] private Animator animator; // Referencia al Animator

    // Método para activar el trigger "Open"
    public void ActivateOpen()
    {
        if (animator != null)
        {
            animator.SetTrigger("Open");
        }
        else
        {
            Debug.LogWarning("⚠️ No hay Animator asignado en " + gameObject.name);
        }
    }
}
