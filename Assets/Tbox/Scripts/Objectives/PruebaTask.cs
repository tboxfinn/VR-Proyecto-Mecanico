using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruebaTask : MonoBehaviour
{
    public TaskObjectiveSO currentTask;

    void Start()
    {
        if (currentTask != null)
        {
            currentTask.InitializeTask(); // Inicializa el Task y sus objetivos
            Debug.Log("Task inicializada: " + currentTask.title);
        }
    }

    void Update()
    {
        // Si haces una acción específica que completa un objetivo, llamas al método
        if (Input.GetKeyDown(KeyCode.Space)) // Ejemplo de acción
        {
            CompleteObjective();
        }
    }

    private void CompleteObjective()
    {
        if (currentTask != null)
        {
            if (!currentTask.IsTaskCompleted())
            {
                currentTask.CompleteCurrentObjective();
                Debug.Log("Completado el objetivo en: " + currentTask.title);
            }
            else
            {
                Debug.Log("Task " + currentTask.title + " ya está completada.");
            }
        }
        else
        {
            Debug.LogError("No se ha asignado un Task para completar.");
        }
    }
}
