using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PruebaTask : MonoBehaviour
{
    public TaskObjectiveSO currentTask;

    void Start()
    {
        if (currentTask != null)
        {
            currentTask.InitializeTask(); // Inicializa el Task y sus objetivos
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
            }
        }
    }

}
