using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PruebaTask : MonoBehaviour
{
    public TaskObjectiveSO currentTask;
    public TMP_Text titleText;         // Texto del título del Task
    public TMP_Text descriptionText;   // Texto de la descripción del objetivo

    void Start()
    {
        if (currentTask != null)
        {
            currentTask.InitializeTask(); // Inicializa el Task y sus objetivos
            UpdateTaskUI();               // Actualiza el UI con el primer objetivo
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
                UpdateTaskUI(); // Actualiza el UI cuando se completa un objetivo
            }
        }
    }

    // Actualiza el UI con el objetivo actual
    private void UpdateTaskUI()
    {
        if (currentTask != null && descriptionText != null)
        {
            ObjectiveSO currentObjective = currentTask.GetCurrentObjective();
            if (currentObjective != null)
            {
                descriptionText.text = currentObjective.description;
            }
            else
            {
                descriptionText.text = "Task Completed!";
            }
        }
    }
}
