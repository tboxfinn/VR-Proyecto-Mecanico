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
        if (debugCompleteObjective)
        {
            CompleteCurrentObjective();
            debugCompleteObjective = false;
        }
    }

    // Método para completar un objetivo en un task específico
    private void CompleteObjective(TaskObjectiveSO task)
    {
        if (task != null)
        {
            ObjectiveSO currentObjective = task.GetCurrentObjective();
            if (currentObjective != null)
            {
                task.CompleteCurrentObjective();
                Debug.Log("Objetivo completado en Task: " + task.taskName);

                // Verificar si la Task está completada
                if (task.IsTaskCompleted())
                {
                    Debug.Log("Task completada: " + task.taskName);
                    MoveToNextTask(); // Mover al siguiente task si hay uno
                }
            }
            else
            {
                Debug.LogError("No hay objetivo actual en la Task seleccionada.");
            }
        }
        else
        {
            Debug.LogError("Task no puede ser nulo.");
        }
    }

    public void CompleteObjectiveByTaskName(string taskName)
    {
        // Buscar el Task en la lista de tasks seleccionados
        TaskObjectiveSO taskToComplete = selectedTasks.Find(task => task.taskName.Equals(taskName, System.StringComparison.OrdinalIgnoreCase));

        if (taskToComplete != null)
        {
            CompleteObjective(taskToComplete); // Llama al método que completa el objetivo
        }
        else
        {
            Debug.LogError("No se encontró el Task con nombre: " + taskName);
        }
    }

    private void CompleteCurrentObjective()
    {
        if (currentTask != null && !currentTask.IsTaskCompleted())
        {
            ObjectiveSO currentObjective = currentTask.GetCurrentObjective();
            
            if (currentObjective != null)
            {
                currentTask.CompleteCurrentObjective();
                Debug.Log("Objetivo completado: " + currentObjective.Title);

                if (currentTask.IsTaskCompleted())
                {
                    Debug.Log("Task completada: " + currentTask.taskName);
                    MoveToNextTask();
                }
            }
            else
            {
                Debug.LogError("No hay objetivo actual para completar.");
            }
        }
    }

    private void MoveToNextTask()
    {
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

    // Método para establecer el Task actual e inicializar sus objetivos
    private void SetCurrentTask(TaskObjectiveSO task)
    {
        if (task != null)
        {
            currentTask = task;
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

    public List<TaskObjectiveSO> ActiveTasks => selectedTasks;
}
