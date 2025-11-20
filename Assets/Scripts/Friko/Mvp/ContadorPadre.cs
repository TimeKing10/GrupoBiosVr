using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ContadorPadre : MonoBehaviour
{
    [Header("Sockets del padre")]
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor[] sockets;  // 3 sockets

    public int contador = 0;
    public int maxContador = 3;

    private void Start()
    {
        foreach (var socket in sockets)
        {
            socket.selectEntered.AddListener(OnObjectPlaced);
            socket.selectExited.AddListener(OnObjectRemoved);
        }
    }

    void Update()
    {
        print("Contador actual: " + contador);
    }

    private void OnDestroy()
    {
        foreach (var socket in sockets)
        {
            socket.selectEntered.RemoveListener(OnObjectPlaced);
            socket.selectExited.RemoveListener(OnObjectRemoved);
        }
    }

    void OnObjectPlaced(SelectEnterEventArgs args)
    {
        if (contador < maxContador)
            contador++;
    }

    void OnObjectRemoved(SelectExitEventArgs args)
    {
        if (contador > 0)
            contador--;
    }
}
