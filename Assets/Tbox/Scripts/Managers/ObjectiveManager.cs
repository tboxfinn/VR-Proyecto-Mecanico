using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    // Lista de todos los Task disponibles
    public List<TaskObjectiveSO> availableTasks = new List<TaskObjectiveSO>();

    // Lista de Task seleccionados que se utilizarán
    [SerializeField] private List<TaskObjectiveSO> selectedTasks = new List<TaskObjectiveSO>();

    private int currentTaskIndex = 0; // Índice de la Task actual en la lista de seleccionados
    private TaskObjectiveSO currentTask; // Task actualmente en progreso

    // Cantidad mínima y máxima de Tasks que queremos seleccionar
    public int minRandomTasks;
    public int maxRandomTasks;

    public bool debugCompleteObjective;

    void Start()
    {
        // Seleccionar Tasks aleatorios de la lista de availableTasks
        SelectRandomTasks();

        // Escoger el primer Task de la lista seleccionada (si hay alguno)
        if (selectedTasks.Count > 0)
        {
            SetCurrentTask(selectedTasks[currentTaskIndex]);
        }
        else
        {
            Debug.LogError("No hay Tasks seleccionados.");
        }
    }

    void Update()
    {
        if (debugCompleteObjective && currentTask != null && !currentTask.IsTaskCompleted())
        {
            ObjectiveSO currentObjective = currentTask.GetCurrentObjective();

            // Completar el objetivo actual
            currentTask.CompleteCurrentObjective();
            Debug.Log("Objetivo completado: " + currentObjective.Title);

            // Resetear el bool de debug
            debugCompleteObjective = false;

            // Verificar si la Task está completada
            if (currentTask.IsTaskCompleted())
            {
                Debug.Log("Task completada: " + currentTask.taskName);

                // Ir a la siguiente Task si hay más disponibles
                currentTaskIndex++;
                if (currentTaskIndex < selectedTasks.Count)
                {
                    SetCurrentTask(selectedTasks[currentTaskIndex]);
                }
                else
                {
                    Debug.Log("¡Todos los Tasks seleccionados han sido completados!");
                }
            }
        }
    }

    // Método para establecer el Task actual e inicializar sus objetivos
    private void SetCurrentTask(TaskObjectiveSO task)
    {
        if (task != null)
        {
            currentTask = Instantiate(task);
            currentTask.InitializeTask();
            Debug.Log("Iniciando Task: " + currentTask.taskName);
        }
    }

    // Seleccionar aleatoriamente un número de Tasks de la lista availableTasks
    private void SelectRandomTasks()
    {
        // Asegurarse de no seleccionar más Tasks de las que hay disponibles
        int maxTasksToSelect = Mathf.Min(maxRandomTasks, availableTasks.Count);

        // Elegir aleatoriamente una cantidad entre el mínimo y el máximo
        int countToSelect = Random.Range(minRandomTasks, maxTasksToSelect + 1);

        // Crear una lista temporal para evitar duplicados
        List<TaskObjectiveSO> tempTaskList = new List<TaskObjectiveSO>(availableTasks);

        // Limpiar la lista de Tasks seleccionados
        selectedTasks.Clear();

        // Seleccionar aleatoriamente las Tasks
        for (int i = 0; i < countToSelect; i++)
        {
            int randomIndex = Random.Range(0, tempTaskList.Count);
            selectedTasks.Add(tempTaskList[randomIndex]);
            tempTaskList.RemoveAt(randomIndex); // Remover la Task para evitar seleccionar la misma
        }

        Debug.Log(countToSelect + " Tasks seleccionadas aleatoriamente.");
    }

    // Lógica para verificar si un objetivo está completado
    private bool CheckIfObjectiveCompleted(ObjectiveSO objective)
    {
        return objective.isCompleted;
    }
}
