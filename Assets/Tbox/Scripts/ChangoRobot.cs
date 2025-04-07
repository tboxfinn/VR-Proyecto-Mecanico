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

    public void Start()
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
            WalkTo(targetPosition.transform.position); // Usa la posición del objeto objetivo
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
        while (navMeshAgent != null && navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
        {
            yield return null; // Espera un frame
        }

        // Cuando llegue al destino, desactiva el bool de caminando
        isWalking = false;

        if (animator != null)
        {
            animator.SetBool("isWalking", false); // Detiene la animación de caminar
        }

        Debug.Log("ChangoRobot ha llegado al destino.");

        // Gira hacia la cámara
        RotateTowardsCamera();
    }

    private void RotateTowardsCamera()
    {
        if (Camera.main != null)
        {
            Vector3 direction = (Camera.main.transform.position - transform.position).normalized; // Calcula la dirección hacia la cámara
            direction.y = 0; // Ignora la rotación en el eje Y para evitar inclinaciones
            Quaternion targetRotation = Quaternion.LookRotation(direction); // Calcula la rotación hacia la cámara
            transform.rotation = targetRotation; // Aplica la rotación
            Debug.Log("ChangoRobot está mirando hacia la cámara.");
        }
        else
        {
            Debug.LogWarning("No se encontró la cámara principal.");
        }
    }

    public void ReturnToStartPosition()
    {
        // Llama a WalkTo con la posición inicial
        WalkTo(startPosition);
    }
}
