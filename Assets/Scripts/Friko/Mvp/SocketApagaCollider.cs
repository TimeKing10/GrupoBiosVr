using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketApagaCollider : MonoBehaviour
{
    UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socket;

    private void Awake()
    {
        socket = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>();
    }

    private void OnEnable()
    {
        socket.selectEntered.AddListener(OnObjectPlaced);
        socket.selectExited.AddListener(OnObjectRemoved);
    }

    private void OnDisable()
    {
        socket.selectEntered.RemoveListener(OnObjectPlaced);
        socket.selectExited.RemoveListener(OnObjectRemoved);
    }

    private void OnObjectPlaced(SelectEnterEventArgs args)
    {
        GameObject obj = args.interactableObject.transform.gameObject;

        BoxCollider col = obj.GetComponent<BoxCollider>();
        if (col != null)
        {
            col.enabled = false;   // ❌ Apagar collider
        }
    }

    private void OnObjectRemoved(SelectExitEventArgs args)
    {
        GameObject obj = args.interactableObject.transform.gameObject;

        BoxCollider col = obj.GetComponent<BoxCollider>();
        if (col != null)
        {
            col.enabled = true;    // ✅ Encender collider
        }
    }
}
