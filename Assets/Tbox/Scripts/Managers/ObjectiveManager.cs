using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    public List<ObjectiveSO> allObjectives; // List of all possible objectives
    public List<ObjectiveSO> activeObjectives; // List of active objectives

    public int minObjectives;
    public int maxObjectives;

    public GameObject objectiveUIPrefab; // Prefab for displaying an objective
    public Transform objectiveUIParent; // Parent transform for the objective UI elements

    void Start()
    {
        InitializeRandomObjectives(minObjectives, maxObjectives);
        UpdateObjectiveUI();
    }

    public void CompleteObjective(ObjectiveSO objective)
    {
        if (activeObjectives.Contains(objective))
        {
            objective.isCompleted = true;

            // Check if the objective is a TaskObjectiveSO and if all steps are completed
            if (objective is TaskObjectiveSO taskObjective)
            {
                if (taskObjective.IsTaskCompleted())
                {
                    taskObjective.isCompleted = true;
                }
            }

            UpdateObjectiveUI();
        }
    }

    private void InitializeRandomObjectives(int minObjectives, int maxObjectives)
    {
        activeObjectives = new List<ObjectiveSO>();
        int numberOfObjectives = Random.Range(minObjectives, maxObjectives + 1); // Random number between min and max

        for (int i = 0; i < numberOfObjectives; i++)
        {
            ObjectiveSO randomObjective = GetRandomObjective();
            if (randomObjective != null)
            {
                activeObjectives.Add(randomObjective);
                AddObjectiveToUI(randomObjective);
            }
        }
    }

    private ObjectiveSO GetRandomObjective()
    {
        if (allObjectives.Count == 0)
        {
            return null;
        }

        int randomIndex = Random.Range(0, allObjectives.Count);
        return allObjectives[randomIndex];
    }

    private void UpdateObjectiveUI()
    {
        // Clear existing UI elements
        foreach (Transform child in objectiveUIParent)
        {
            Destroy(child.gameObject);
        }

        // Create new UI elements for each active objective
        foreach (ObjectiveSO objective in activeObjectives)
        {
            AddObjectiveToUI(objective);
        }
    }

    private void AddObjectiveToUI(ObjectiveSO objective)
    {
        GameObject objectiveUI = Instantiate(objectiveUIPrefab, objectiveUIParent);
        TMP_Text objectiveText = objectiveUI.GetComponent<TMP_Text>();
        if (objectiveText != null)
        {
            if (objective is TaskObjectiveSO taskObjective)
            {
                objectiveText.text = taskObjective.description + (taskObjective.IsTaskCompleted() ? " (Completed)" : "");
                foreach (ObjectiveSO step in taskObjective.steps)
                {
                    GameObject stepUI = Instantiate(objectiveUIPrefab, objectiveUIParent);
                    TMP_Text stepText = stepUI.GetComponent<TMP_Text>();
                    if (stepText != null)
                    {
                        stepText.text = " - " + step.description + (step.isCompleted ? " (Completed)" : "");
                    }
                }
            }
            else
            {
                objectiveText.text = objective.description + (objective.isCompleted ? " (Completed)" : "");
            }
        }
    }

    public void AddNewObjective(ObjectiveSO newObjective)
    {
        activeObjectives.Add(newObjective);
        AddObjectiveToUI(newObjective);
    }

    // Methods to modify steps in a TaskObjectiveSO
    public void AddStepToTask(TaskObjectiveSO taskObjective, ObjectiveSO newStep)
    {
        taskObjective.steps.Add(newStep);
        UpdateObjectiveUI();
    }

    public void RemoveStepFromTask(TaskObjectiveSO taskObjective, ObjectiveSO stepToRemove)
    {
        taskObjective.steps.Remove(stepToRemove);
        UpdateObjectiveUI();
    }

    public void UpdateStepInTask(TaskObjectiveSO taskObjective, ObjectiveSO stepToUpdate, string newDescription, bool isCompleted)
    {
        int index = taskObjective.steps.IndexOf(stepToUpdate);
        if (index != -1)
        {
            taskObjective.steps[index].description = newDescription;
            taskObjective.steps[index].isCompleted = isCompleted;
            UpdateObjectiveUI();
        }
    }
}