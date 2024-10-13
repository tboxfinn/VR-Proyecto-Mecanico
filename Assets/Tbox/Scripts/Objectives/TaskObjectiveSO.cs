using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Task Objective", menuName = "Objective/Task Objective")]
public class TaskObjectiveSO : ScriptableObject
{
    public string taskName;
    public ObjectiveSO[] objectives; // Referencia a los objetivos originales

    private ObjectiveSO[] instancedObjectives; // Copias de los objetivos para no modificar los originales
    private int currentObjectiveIndex = 0;

    // Método para instanciar los objetivos y trabajar con copias
    public void InitializeTask()
    {
        // Instancia los objetivos del Task
        instancedObjectives = new ObjectiveSO[objectives.Length];
        for (int i = 0; i < objectives.Length; i++)
        {
            instancedObjectives[i] = Instantiate(objectives[i]);
        }
    }

    public bool IsTaskCompleted()
    {
        return currentObjectiveIndex >= instancedObjectives.Length;
    }

    public ObjectiveSO GetCurrentObjective()
    {
        if (currentObjectiveIndex < instancedObjectives.Length)
        {
            return instancedObjectives[currentObjectiveIndex];
        }

        return null;
    }

    public void CompleteCurrentObjective()
    {
        if (currentObjectiveIndex < instancedObjectives.Length)
        {
            instancedObjectives[currentObjectiveIndex].CompleteObjective();
            currentObjectiveIndex++;
        }
    }
}