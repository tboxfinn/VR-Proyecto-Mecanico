using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class ChangoRobot : MonoBehaviour
{
    public bool conversationHasStarted = false;
    public bool conversationHasEnded = false;

    public NPCConversation myConversation;

    [Header("Movement")]
    public bool isWalking = false; // Bool para indicar si está caminando
    public float moveSpeed = 2f; // Velocidad de movimiento
    public Animator animator; // Referencia al Animator para controlar la animación

    public GameObject targetPosition; // Referencia al objeto objetivo desde la escena
    private bool shouldMove = false; // Controla si debe moverse
    private Vector3 startPosition; // Posición inicial del ChangoRobot

    private void Start()
    {
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

    public void WalkTo(Vector3 position)
    {
        shouldMove = true; // Activa el movimiento
        isWalking = true; // Activa el bool de caminando

        if (animator != null)
        {
            animator.SetBool("isWalking", true); // Activa la animación de caminar
        }

        targetPosition = null; // Limpia la referencia para evitar conflictos
        transform.position = Vector3.MoveTowards(transform.position, position, moveSpeed * Time.deltaTime);
    }

    public void ReturnToStartPosition()
    {
        // Llama a WalkTo con la posición inicial
        WalkTo(startPosition);
    }

    private void Update()
    {
        if (shouldMove)
        {
            // Mueve al ChangoRobot hacia la posición objetivo
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.transform.position, moveSpeed * Time.deltaTime);

            // Verifica si ha llegado al destino
            if (Vector3.Distance(transform.position, targetPosition.transform.position) <= 0.1f)
            {
                shouldMove = false; // Detiene el movimiento
                isWalking = false; // Desactiva el bool de caminando

                if (animator != null)
                {
                    animator.SetBool("isWalking", false); // Detiene la animación de caminar
                }

                Debug.Log("ChangoRobot ha llegado al destino.");
            }
        }
    }
}
