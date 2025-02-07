using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TaskStep
{
    public string description;
    public List<GameObject> objectsToOutline;
}

public class TaskStepController : MonoBehaviour
{
    public TaskObjectiveSO targetTask; // Task que se va a completar por pasos
    [Tooltip("Lista de pasos para completar el task en orden.")]
    public List<TaskStep> taskSteps; // Descripciones de los pasos específicos
    private int currentStepIndex = 0; // Índice del paso actual que se espera completar

    public delegate void OnStepCompleted(string step);
    public event OnStepCompleted StepCompletedEvent;

    public virtual void Start()
    {
        if (targetTask != null)
        {
            targetTask.InitializeTask(); // Asegura que el task esté inicializado
            ActivateCurrentStepOutlines();
        }
    }

    public virtual void Update()
    {
        
    }

    public void CompleteStep(string step)
    {
        // Verifica si el paso actual es el esperado en el orden correcto
        if (currentStepIndex < taskSteps.Count && taskSteps[currentStepIndex].description == step && targetTask != null)
        {
            StepCompletedEvent?.Invoke(step); // Opcional: notificar la finalización del paso
            DeactivateCurrentStepObjects();
            currentStepIndex++;
            ActivateCurrentStepOutlines();

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
        return currentStepIndex < taskSteps.Count ? taskSteps[currentStepIndex].description : "Todos los pasos completados";
    }

    public void ResetSteps()
    {
        DeactivateCurrentStepObjects();
        currentStepIndex = 0;
        ActivateCurrentStepOutlines();
    }

    private void ActivateCurrentStepOutlines()
    {
        if (currentStepIndex < taskSteps.Count && targetTask.isVisible)
        {
            foreach (GameObject obj in taskSteps[currentStepIndex].objectsToOutline)
            {
                var outline = obj.GetComponent<Outline>();
                if (outline != null)
                {
                    outline.enabled = true;
                }
            }
        }
    }

    private void DeactivateCurrentStepObjects()
    {
        if (currentStepIndex < taskSteps.Count)
        {
            foreach (var obj in taskSteps[currentStepIndex].objectsToOutline)
            {
                var outline = obj.GetComponent<Outline>();
                if (outline != null)
                {
                    outline.enabled = false;
                }
            }
        }
    }
}
