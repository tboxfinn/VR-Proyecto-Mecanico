using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor; // Asegúrate de que la ruta del espacio de nombres sea correcta para tu proyecto

public class ChangoRobot : MonoBehaviour
{
    public bool conversationHasStarted = false;
    public bool conversationHasEnded = false;

    public NPCConversation myConversation;

    [Header("Movement")]
    public Animator animator; // Referencia al Animator para controlar la animación
    public float walkDuration = 2f; // Duración del tiempo que caminará el robot (configurable desde el Inspector)

    [Header("Animation")]
    public GameObject[] objectsToActivate; // Arreglo de objetos que se pueden activar

    [Header("Raycast")]
    public float rayDistance = 5f; // Distancia del raycast
    public LayerMask rayLayerMask; // Capas con las que interactúa el raycast
    

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>(); // Obtén el Animator si no está asignado
        }

        if (ConversationManager.Instance != null)
        {
            ConversationManager.Instance.SetBool("Regado", true);
        }
        else
        {
            Debug.LogError("ConversationManager.Instance es null. Asegúrate de que el ConversationManager esté en la escena.");
        }
    }

    private void Update()
    {
        // Lanza el raycast desde el frente del robot
        PerformRaycast();
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
        }
    }

    public void ActivateObject(int index)
    {
        if (index >= 0 && index < objectsToActivate.Length)
        {
            objectsToActivate[index].SetActive(true); // Activa el objeto en el índice especificado
            
            Debug.Log($"Objeto activado: {objectsToActivate[index].name}");
        }
        else
        {
            Debug.LogWarning("Índice fuera de rango. No se pudo activar el objeto.");
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

    public void StartConversation()
    {
        if (!conversationHasStarted)
        {
            conversationHasStarted = true;
            ConversationManager.Instance.StartConversation(myConversation);
        }
    }
}
