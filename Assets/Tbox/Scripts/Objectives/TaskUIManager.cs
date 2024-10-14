using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaskUIManager : MonoBehaviour
{
    public List<TaskObjectiveSO> allTasks; // Lista de todos los Task disponibles
    public int minTasks = 1; // Mínimo de Task que se seleccionarán
    public int maxTasks = 3; // Máximo de Task que se seleccionarán
    public GameObject taskUIPrefab; // Prefab con los textos de título y descripción
    public Transform taskUIHolder; // Holder donde se instanciarán los prefabs

    [SerializeField] private List<TaskObjectiveSO> selectedTasks; // Lista de Tasks seleccionados

    private List<GameObject> instantiatedPrefabs = new List<GameObject>();

    void Start()
    {
        // Selecciona una cantidad aleatoria de Task
        SelectRandomTasks();

        // Instancia los Tasks seleccionados en el UI
        InstantiateTaskUI();
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
        foreach (TaskObjectiveSO task in selectedTasks)
        {
            // Inicializa el Task antes de tomar el primer objetivo
            task.InitializeTask();

            // Instancia el prefab en el holder
            GameObject taskUIInstance = Instantiate(taskUIPrefab, taskUIHolder);
            instantiatedPrefabs.Add(taskUIInstance);

            // Busca los objetos de texto dentro del prefab instanciado
            TMP_Text titleText = taskUIInstance.transform.Find("TaskTitle").GetComponent<TMP_Text>();
            TMP_Text descriptionText = taskUIInstance.transform.Find("ObjectiveDescription").GetComponent<TMP_Text>();

            // Asigna el título y la descripción del Task activo
            titleText.text = task.title;
            descriptionText.text = task.GetCurrentObjective()?.description;

            // Suscríbete al evento para actualizar la UI cuando un objetivo sea completado
            task.ObjectiveCompletedEvent += () => UpdateTaskUI(task, taskUIInstance);
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
        }
        else
        {
            descriptionText.text = task.GetCurrentObjective()?.description;
        }
    }
}
