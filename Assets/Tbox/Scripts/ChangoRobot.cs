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
    public Animator animator; // Referencia al Animator para controlar la animación
    public float walkDuration = 2f; // Duración del tiempo que caminará el robot (configurable desde el Inspector)

    [Header("Animation")]
    public GameObject objectToActivate; // Objeto que se activará al mirar el robot

    [Header("Raycast")]
    public float rayDistance = 5f; // Distancia del raycast
    public LayerMask rayLayerMask; // Capas con las que interactúa el raycast

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>(); // Obtén el Animator si no está asignado
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

    public void ActivateObject()
    {
        objectToActivate.SetActive(true); // Activa el objeto al mirar el robot
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
