using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public TaskObjectiveSO originalTask; // Referencia al Task original
    [SerializeField] private TaskObjectiveSO currentTask; // Copia instanciada del Task

    public bool debugCompleteObjective;

    void Start()
    {
        if (originalTask != null)
        {
            // Instancia una copia del Task y sus objetivos
            currentTask = Instantiate(originalTask);
            currentTask.InitializeTask(); // Inicializa las copias de los objetivos
        }
    }

    void Update()
    {
        if (debugCompleteObjective && currentTask != null && !currentTask.IsTaskCompleted())
        {
            ObjectiveSO currentObjective = currentTask.GetCurrentObjective();

            // Completa el objetivo actual
            currentTask.CompleteCurrentObjective();
            Debug.Log("Objetivo completado: " + currentObjective.Title);

            // Resetear el bool de debug
            debugCompleteObjective = false;
        }
    }

    private bool CheckIfObjectiveCompleted(ObjectiveSO objective)
    {
        // Aquí puedes definir la lógica específica para verificar si un objetivo se ha completado
        return objective.isCompleted;
    }
}