using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TaskUIManager : MonoBehaviour
{
    public List<TaskObjectiveSO> allTasks; // Lista de todos los Task disponibles
    public int minTasks = 1; // Mínimo de Task que se seleccionarán
    public int maxTasks = 3; // Máximo de Task que se seleccionarán
    public int taskOnUI = 3;
    public GameObject taskUIPrefab; // Prefab con los textos de título y descripción
    public Transform taskUIHolder; // Holder donde se instanciarán los prefabs

    [SerializeField] private List<TaskObjectiveSO> selectedTasks; // Lista de Tasks seleccionados

    private List<GameObject> instantiatedPrefabs = new List<GameObject>();

    void Start()
    {
        // Resetear la visibilidad de todas las tareas
        ResetAllTasksVisibility();

        // Selecciona una cantidad aleatoria de Task
        SelectRandomTasks();

        // Instancia los Tasks seleccionados en el UI
        InstantiateTaskUI();
    }

    void ResetAllTasksVisibility()
    {
        foreach (var task in allTasks)
        {
            task.isVisible = false;
        }
    }

    void SelectRandomTasks()
    {
        selectedTasks = new List<TaskObjectiveSO>();

        // Asegúrate de no seleccionar más Task de los que existen en la lista
        int numberOfTasksToSelect = Random.Range(minTasks, Mathf.Min(maxTasks + 1, allTasks.Count + 1));

        List<TaskObjectiveSO> tempTasks = new List<TaskObjectiveSO>(allTasks);

        // Selecciona Tasks aleatoriamente
        for (int i = 0; i < numberOfTasksToSelect; i++)
        {
            int randomIndex = Random.Range(0, tempTasks.Count);
            selectedTasks.Add(tempTasks[randomIndex]);
            tempTasks.RemoveAt(randomIndex); // Remueve el Task ya seleccionado
        }
    }

    void InstantiateTaskUI()
    {
        int tasksToInstantiate = Mathf.Min(selectedTasks.Count, taskOnUI); // Limita a 3 tareas

        for (int i = 0; i < tasksToInstantiate; i++)
        {
            TaskObjectiveSO task = selectedTasks[0]; // Toma el primer task de la lista

            // Inicializa el Task antes de tomar el primer objetivo
            task.InitializeTask();
            task.isVisible = true;

            // Instancia el prefab en el primer holder
            GameObject taskUIInstance = Instantiate(taskUIPrefab, taskUIHolder);
            instantiatedPrefabs.Add(taskUIInstance);

            // Busca los objetos de texto dentro del primer prefab instanciado
            TMP_Text titleText = taskUIInstance.transform.Find("TaskTitle").GetComponent<TMP_Text>();
            TMP_Text descriptionText = taskUIInstance.transform.Find("ObjectiveDescription").GetComponent<TMP_Text>();
            Image taskImage = taskUIInstance.transform.Find("TaskImage").GetComponent<Image>();

            // Asigna el título del Task activo si no es nulo
            if (titleText != null)
            {
                titleText.text = task.title ?? "No Title";
            }

            // Asigna la descripción del Task activo si no es nula
            if (descriptionText != null)
            {
                descriptionText.text = task.GetCurrentObjective()?.description ?? "No Description";
            }

            // Asigna la imagen del Task activo si no es nula
            if (taskImage != null)
            {
                taskImage.sprite = task.taskImage ?? null; // Puedes asignar una imagen por defecto si es nula
            }

            EnableOutline(task);

            // Suscríbete al evento para actualizar la UI cuando un objetivo sea completado
            task.ObjectiveCompletedEvent += () => UpdateTaskUI(task, taskUIInstance);

            // Elimina el task de la lista de selectedTasks
            selectedTasks.RemoveAt(0);
        }
    }

    void UpdateTaskUI(TaskObjectiveSO task, GameObject taskUIInstance)
    {
        TMP_Text titleText = taskUIInstance.transform.Find("TaskTitle").GetComponent<TMP_Text>();
        TMP_Text descriptionText = taskUIInstance.transform.Find("ObjectiveDescription").GetComponent<TMP_Text>();

        // Actualiza la descripción con el siguiente objetivo o indica que el Task está completado
        if (task.IsTaskCompleted())
        {   
            // Cambia el estilo de la fuente al completar el Task
            titleText.fontStyle = FontStyles.Strikethrough;
            descriptionText.fontStyle = FontStyles.Strikethrough;
            task.isVisible = false;

            DisableOutline(task);

            SoundManager.instance.PlaySound("TaskCompleted");

            // Actualiza la UI para mostrar la siguiente tarea si hay más tareas disponibles
            if (selectedTasks.Count > 0)
            {
                // Inicia la corrutina para mostrar la siguiente tarea después de un retraso
                StartCoroutine(ShowNextTaskWithDelay());
            }
        }
        else
        {
            descriptionText.text = task.GetCurrentObjective()?.description;
        }
    }

    IEnumerator ShowNextTaskWithDelay()
    {
        // Espera un breve momento antes de cambiar a la siguiente tarea
        yield return new WaitForSeconds(1.0f);

        ShowNextTask();
    }

    void ShowNextTask()
    {
        // Encuentra el primer prefab completado y destrúyelo
        for (int i = 0; i < instantiatedPrefabs.Count; i++)
        {
            TMP_Text titleText = instantiatedPrefabs[i].transform.Find("TaskTitle").GetComponent<TMP_Text>();
            if (titleText.fontStyle == FontStyles.Strikethrough)
            {
                Destroy(instantiatedPrefabs[i]);
                instantiatedPrefabs.RemoveAt(i);
                break;
            }
        }

        // Instancia la siguiente tarea si hay más tareas disponibles
        if (selectedTasks.Count > 0)
        {
            TaskObjectiveSO nextTask = selectedTasks[0];

            // Inicializa el Task antes de tomar el primer objetivo
            nextTask.InitializeTask();

            // Instancia el prefab en el holder
            GameObject taskUIInstance = Instantiate(taskUIPrefab, taskUIHolder);
            instantiatedPrefabs.Add(taskUIInstance);

            // Busca los objetos de texto dentro del prefab instanciado
            TMP_Text newTitleText = taskUIInstance.transform.Find("TaskTitle").GetComponent<TMP_Text>();
            TMP_Text newDescriptionText = taskUIInstance.transform.Find("ObjectiveDescription").GetComponent<TMP_Text>();
            Image newTaskImage = taskUIInstance.transform.Find("TaskImage").GetComponent<Image>();

            // Actualiza los textos con la información del nuevo Task
            newTitleText.text = nextTask.title;
            newDescriptionText.text = nextTask.GetCurrentObjective()?.description;

            if (newTaskImage != null)
            {
                newTaskImage.sprite = nextTask.taskImage ?? null; // Puedes asignar una imagen por defecto si es nula
            }

            EnableOutline(nextTask);

            // Suscríbete al evento para actualizar la UI cuando un objetivo sea completado
            nextTask.ObjectiveCompletedEvent += () => UpdateTaskUI(nextTask, taskUIInstance);

            // Elimina el task de la lista de selectedTasks
            selectedTasks.RemoveAt(0);
        }
    }

    void EnableOutline(TaskObjectiveSO task)
    {
        foreach (var objective in task.objectives)
        {
            foreach (var obj in objective.objectsToOutline)
            {
                var outline = obj.GetComponent<Outline>();
                if (outline != null)
                {
                    outline.enabled = true;
                }
            }
        }
    }

    void DisableOutline(TaskObjectiveSO task)
    {
        foreach (var objective in task.objectives)
        {
            foreach (var obj in objective.objectsToOutline)
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