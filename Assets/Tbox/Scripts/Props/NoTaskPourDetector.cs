using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoTaskPourDetector : MonoBehaviour
{
    [Tooltip("Tag del objeto que quieres detectar.")]
    public string targetTag = "Oil"; // Cambia esto por el tag que quieras detectar

    public UnityEngine.Events.UnityEvent OnTargetEntered; // Evento que se invocará al entrar en contacto con el objeto

    // Este método se llama automáticamente cuando otro objeto con un Collider sale de contacto con este Trigger
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {   
            OnTargetEntered?.Invoke(); // Invoca el evento si hay suscriptores
        }
    }

}
