using UnityEngine;

public class AnimatorTrigger : MonoBehaviour
{
    [SerializeField] private Animator animatorRightDoor; // Referencia al Animator
    [SerializeField] private Animator animatorLeftDoor; // Referencia al Animator

    // Método para activar el trigger "Open"
    public void ActivateOpen()
    {
        if (animatorRightDoor != null && animatorLeftDoor != null)
        {
            animatorRightDoor.SetTrigger("Open");
            animatorLeftDoor.SetTrigger("Open");
        }
        else
        {
            Debug.LogWarning("⚠️ No hay Animator asignado en " + gameObject.name);
        }
    }
}
