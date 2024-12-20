using System.Collections.Generic;
using UnityEngine;

public class TaskStepController : MonoBehaviour
{
    public TaskObjectiveSO targetTask; // Task que se va a completar por pasos
    [Tooltip("Lista de pasos para completar el task en orden.")]
    public List<string> taskSteps; // Descripciones de los pasos específicos
    private int currentStepIndex = 0; // Índice del paso actual que se espera completar

    public delegate void OnStepCompleted(string step);
    public event OnStepCompleted StepCompletedEvent;

    public virtual void Start()
    {
        if (targetTask != null)
        {
            targetTask.InitializeTask(); // Asegura que el task esté inicializado
        }
    }

    public virtual void Update()
    {
        
    }

    public void CompleteStep(string step)
    {
        // Verifica si el paso actual es el esperado en el orden correcto
        if (currentStepIndex < taskSteps.Count && taskSteps[currentStepIndex] == step && targetTask != null)
        {
            StepCompletedEvent?.Invoke(step); // Opcional: notificar la finalización del paso
            currentStepIndex++;

            // Llama a CompleteCurrentObjective del Task solo al completar el paso correcto
            targetTask.CompleteCurrentObjective();

            // Verifica si se completaron todos los pasos
            if (currentStepIndex >= taskSteps.Count)
            {
                //All steps completed
            }
        }
        else
        {
            //Step not completed
        }
    }

    public string GetCurrentStep()
    {
        return currentStepIndex < taskSteps.Count ? taskSteps[currentStepIndex] : "Todos los pasos completados";
    }

    public void ResetSteps()
    {
        currentStepIndex = 0;
    }
}
