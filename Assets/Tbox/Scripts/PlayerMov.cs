using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMov : MonoBehaviour
{
    public NavMeshAgent agent; // Referencia al NavMeshAgent
    public List<Transform> waypoints; // Lista de puntos a los que los agentes pueden ir
    private Transform currentTarget; // Punto actual al que el agente se dirige

    private void Start()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>(); // Obtén el NavMeshAgent si no está asignado
        }

        // Asigna un punto inicial al agente
        AssignNewWaypoint();
    }

    private void Update()
    {
        // Verifica si el agente ha llegado al destino
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                // Asigna un nuevo punto cuando llega al destino
                AssignNewWaypoint();
            }
        }
    }

    private void AssignNewWaypoint()
    {
        // Encuentra un nuevo punto aleatorio
        Transform newTarget = waypoints[Random.Range(0, waypoints.Count)];

        // Asigna el nuevo punto como destino
        currentTarget = newTarget;
        agent.SetDestination(currentTarget.position);
    }
}
