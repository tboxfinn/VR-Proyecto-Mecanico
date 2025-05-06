using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor; // Asegúrate de que la ruta del espacio de nombres sea correcta para tu proyecto

public class ChangoRobot : MonoBehaviour
{
    public bool conversationHasStarted = false;
    public bool conversationHasEnded = false;

    public NPCConversation myConversation;
    public NPCConversation myConversation2; // Conversación adicional (si es necesario)
    public NPCConversation myConversation3; // Conversación adicional (si es necesario)
    public NPCConversation myConversation4; // Conversación adicional (si es necesario)

    private HashSet<int> completedConversations = new HashSet<int>(); // Registro de conversaciones completadas

    [Header("Movement")]
    public Animator animator; // Referencia al Animator para controlar la animación
    public float walkDuration = 2f; // Duración del tiempo que caminará el robot (configurable desde el Inspector) // Arreglo de objetos que se pueden activar

    [Header("Raycast")]
    public float rayDistance = 5f; // Distancia del raycast
    public LayerMask rayLayerMask; // Capas con las que interactúa el raycast

    [Header("Random Trigger Settings")]
    public string[] animationTriggers; // Lista de triggers de animación
    public float triggerInterval = 5f; // Intervalo de tiempo entre activaciones aleatorias

    private float timeSinceLastTrigger = 0f; // Temporizador para controlar el intervalo

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>(); // Obtén el Animator si no está asignado
        }

        ConversationManager.Instance.SetBool("Regado", true); // Establece el parámetro "robot" en 1 al inicio
    }

    private void Update()
    {
        // Lanza el raycast desde el frente del robot
        PerformRaycast();

        // Incrementa el temporizador
        timeSinceLastTrigger += Time.deltaTime;

        // Activa un trigger aleatorio si ha pasado el intervalo
        if (timeSinceLastTrigger >= triggerInterval)
        {
            ActivateRandomTrigger();
            timeSinceLastTrigger = 0f; // Reinicia el temporizador
        }
    }

    public void StartWalking()
    {
        if (animator != null)
        {
            animator.SetBool("isWalking", true); // Cambia el parámetro del Animator para iniciar la animación de caminar
            StartCoroutine(StopWalkingAfterTime()); // Inicia la corrutina para detener el caminar después de un tiempo
        }
    }

    private IEnumerator StopWalkingAfterTime()
    {
        yield return new WaitForSeconds(walkDuration); // Espera la duración configurada
        StopWalking(); // Detiene el caminar
    }

    public void StopWalking()
    {
        if (animator != null)
        {
            animator.SetBool("isWalking", false); // Cambia el parámetro del Animator para detener la animación de caminar
            ActivateAnimation("Turn"); // Activa la animación de inactividad (Idle)
        }
    }

    public void ActivateAnimation(string animationName)
    {
        if (animator != null)
        {
            animator.SetTrigger(animationName); // Activa la animación especificada
            Debug.Log($"Animación activada: {animationName}");
        }
        else
        {
            Debug.LogWarning("Animator no asignado. No se pudo activar la animación.");
        }
    }

    public void ActivateAnimationTrigger(string triggerName)
    {
        if (animator != null)
        {
            // Reinicia el trigger antes de activarlo
            animator.ResetTrigger(triggerName);

            // Activa el trigger
            animator.SetTrigger(triggerName);

            Debug.Log($"Trigger de animación activado: {triggerName}");
        }
        else
        {
            Debug.LogWarning("Animator no asignado. No se pudo activar el trigger.");
        }
    }

    public void ResetAnimationTrigger(string triggerName)
    {
        if (animator != null)
        {
            animator.ResetTrigger(triggerName); // Resetea el trigger especificado
            Debug.Log($"Trigger de animación reseteado: {triggerName}");
        }
        else
        {
            Debug.LogWarning("Animator no asignado. No se pudo resetear el trigger.");
        }
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

    private void ActivateRandomTrigger()
    {
        if (animationTriggers.Length > 0 && animator != null)
        {
            // Selecciona un trigger aleatorio de la lista
            string randomTrigger = animationTriggers[Random.Range(0, animationTriggers.Length)];

            // Activa el trigger
            animator.SetTrigger(randomTrigger);
            Debug.Log($"Trigger aleatorio activado: {randomTrigger}");
        }
        else
        {
            Debug.LogWarning("No hay triggers configurados o el Animator no está asignado.");
        }
    }

    public void StartConversation(int conversationIndex)
    {
        // Verifica si la conversación ya ocurrió
        if (completedConversations.Contains(conversationIndex))
        {
            Debug.LogWarning($"La conversación {conversationIndex} ya se ha producido y no puede repetirse.");
            return;
        }

        NPCConversation selectedConversation = null;

        // Selecciona la conversación según el índice
        switch (conversationIndex)
        {
            case 1:
                selectedConversation = myConversation;
                break;
            case 2:
                selectedConversation = myConversation2;
                break;
            case 3:
                selectedConversation = myConversation3;
                break;
            case 4:
                selectedConversation = myConversation4;
                break;
            default:
                Debug.LogWarning("Índice de conversación inválido.");
                return;
        }

        // Inicia la conversación seleccionada
        if (!conversationHasStarted && selectedConversation != null)
        {
            conversationHasStarted = true;
            ConversationManager.Instance.StartConversation(selectedConversation);
            Debug.Log($"Iniciando conversación: {selectedConversation.name}");

            // Marca la conversación como completada
            completedConversations.Add(conversationIndex);
        }
        else
        {
            Debug.LogWarning("No se pudo iniciar la conversación. Ya comenzó o no es válida.");
        }
    }

    public void StartConversation1()
    {
        StartConversation(1);
    }

    public void StartConversation2()
    {
        StartConversation(2);
    }

    public void StartConversation3()
    {
        StartConversation(3);
    }

    public void StartConversation4()
    {
        StartConversation(4);
    }

    public void ResetConversationState()
    {
        conversationHasStarted = false;
        conversationHasEnded = false;
        Debug.Log("Estado de la conversación reiniciado. Ahora puedes iniciar otra conversación.");
    }
}
