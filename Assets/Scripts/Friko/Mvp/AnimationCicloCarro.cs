using UnityEngine;

public class AnimationCicloCarro : MonoBehaviour
{
    public Animator animator; 

    void Start()
    {
        if (animator != null)
        {
            animator.SetBool("Usar", false);
        }
        else
        {
            Debug.LogWarning("No se asignó el Animator en AnimationCicloCarro.");
        }
    }
}
