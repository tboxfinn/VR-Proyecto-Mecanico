using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // Necesario para usar NavMeshAgent
using DialogueEditor;

public class ChangoRobot : MonoBehaviour
{
    public bool conversationHasStarted = false;
    public bool conversationHasEnded = false;

    public NPCConversation myConversation;

    [Header("Movement")]
    public bool isWalking = false; // Bool para indicar si está caminando
    public NavMeshAgent navMeshAgent; // Referencia al NavMeshAgent
    public Animator animator; // Referencia al Animator para controlar la animación

    public GameObject targetPosition; // Referencia al objeto objetivo desde la escena
    private Vector3 startPosition; // Posición inicial del ChangoRobot
    public GameObject playerPosition; // Referencia al jugador

    [Header("Raycast")]
    public float rayDistance = 5f; // Distancia del raycast
    public LayerMask rayLayerMask; // Capas con las que interactúa el raycast

    private void Start()
    {
        if (navMeshAgent == null)
        {
            navMeshAgent = GetComponent<NavMeshAgent>(); // Obtén el NavMeshAgent si no está asignado
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>(); // Obtén el Animator si no está asignado
        }

        // Guarda la posición inicial al inicio del juego
        startPosition = transform.position;
    }

    private void Update()
    {
        // Lanza el raycast desde el frente del robot
        PerformRaycast();
    }

    private void PerformRaycast()
    {
        // Dirección del raycast (frente del robot)
        Vector3 rayDirection = transform.forward;

        // Origen del raycast (posición del robot)
        Vector3 rayOrigin = transform.position + Vector3.up * 1f; // Ajusta la altura del rayo si es necesario

        // Lanza el raycast
        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, rayDistance, rayLayerMask))
        {
            Debug.Log($"Raycast hit: {hit.collider.name} at distance {hit.distance}");
        }

        // Dibuja el raycast en la escena
        Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.red);
    }

    public void StartConversation()
    {
        if (!conversationHasStarted)
        {
            conversationHasStarted = true;
            ConversationManager.Instance.StartConversation(myConversation);
        }
    }

    public void WalkToTarget()
    {
        if (targetPosition != null)
        {
            WalkTo(targetPosition.transform.position);
            animator.SetBool("isWalking", true); // Activa la animación de caminar
        }
        else
        {
            Debug.LogWarning("TargetPosition no está asignado.");
        }
    }

    public void WalkTo(Vector3 targetPosition)
    {
        if (navMeshAgent != null)
        {
            isWalking = true; // Activa el bool de caminando
            navMeshAgent.SetDestination(targetPosition); // Establece el destino del NavMeshAgent

            if (animator != null)
            {
                animator.SetBool("isWalking", true); // Activa la animación de caminar
            }

            StartCoroutine(CheckIfArrived(targetPosition)); // Inicia una corrutina para verificar si llegó al destino
        }
        else
        {
            Debug.LogWarning("NavMeshAgent no está asignado en el ChangoRobot.");
        }
    }

    private IEnumerator CheckIfArrived(Vector3 targetPosition)
    {
        // Espera hasta que el NavMeshAgent tenga un camino válido y comience a moverse
        while (navMeshAgent != null && (!navMeshAgent.hasPath || navMeshAgent.velocity.magnitude <= 0))
        {
            yield return null; // Espera un frame
        }

        // Verifica continuamente si ha llegado al destino
        while (navMeshAgent != null && navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
        {
            yield return null; // Espera un frame
        }

        // Cuando llegue al destino, detén el movimiento y la animación
        isWalking = false;

        if (animator != null)
        {
            animator.SetBool("isWalking", false); // Detiene la animación de caminar
        }

        RotateTowardsPlayer(playerPosition.transform); // Gira hacia el jugador

        Debug.Log("ChangoRobot ha llegado al destino.");
    }

    public void ReturnToStartPosition()
    {
        // Llama a WalkTo con la posición inicial
        WalkTo(startPosition);
    }

    public void RotateTowardsPlayer(Transform playerTransform)
    {
        if (playerTransform != null && navMeshAgent != null)
        {
            // Desactiva la rotación automática del NavMeshAgent
            navMeshAgent.updateRotation = false;

            // Calcula la dirección hacia el jugador
            Vector3 direction = (playerTransform.position - transform.position).normalized;

            // Ignora la rotación en el eje Y para evitar inclinaciones
            direction.y = 0;

            // Calcula la rotación hacia el jugador
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Aplica la rotación suavemente
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2f); // Ajusta el factor para controlar la velocidad

            Debug.Log("ChangoRobot está rotando suavemente hacia el jugador.");
        }
        else
        {
            Debug.LogWarning("No se encontró la referencia al jugador o NavMeshAgent no está asignado.");
        }
    }
}
