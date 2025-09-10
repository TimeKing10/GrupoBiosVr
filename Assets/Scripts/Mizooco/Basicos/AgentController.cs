using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour
{
    private NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Método público para cambiar la velocidad
    public void SetAgentSpeed(float newSpeed)
    {
        if (agent != null)
            agent.speed = newSpeed;
    }
}
