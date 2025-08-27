using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class GruaNavMesh : MonoBehaviour
{
    [Header("NavMesh")]
    public NavMeshAgent agent;

    [Header("Puntos de ruta")]
    public Transform puntoRecoger;
    public Transform puntoEntregar;

    [Header("Caja")]
    public GameObject caja; // La caja que se activa/desactiva

    public bool tieneCaja = false;
    private Transform destinoActual;
    private bool esperando = false;

    void Start()
    {
        if (tieneCaja)
        {
            caja.SetActive(true);
            destinoActual = puntoEntregar;
        }
        else
        {
            caja.SetActive(false);
            destinoActual = puntoRecoger;
        }

        agent.SetDestination(destinoActual.position);
    }


    void Update()
    {
        if (!esperando && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            StartCoroutine(EsperarEnPunto());
        }
    }

    IEnumerator EsperarEnPunto()
    {
        esperando = true;

        // Parar el agente
        agent.isStopped = true;

        // Esperar 2 segundos en el punto
        yield return new WaitForSeconds(2f);

        if (!tieneCaja)
        {
            // En punto de recogida
            caja.SetActive(true);
            tieneCaja = true;
            destinoActual = puntoEntregar;
        }
        else
        {
            // En punto de entrega
            caja.SetActive(false);
            tieneCaja = false;
            destinoActual = puntoRecoger;
        }

        // Reanudar movimiento
        agent.isStopped = false;
        agent.SetDestination(destinoActual.position);

        esperando = false;
    }
}
