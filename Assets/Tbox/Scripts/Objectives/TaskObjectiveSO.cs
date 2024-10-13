using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Task Objective", menuName = "Objective System/Task Objective")]
public class TaskObjectiveSO : ObjectiveSO
{
    public List<ObjectiveSO> steps; // List of steps for this task

    public bool IsTaskCompleted()
    {
        foreach (ObjectiveSO step in steps)
        {
            if (!step.isCompleted)
            {
                return false;
            }
        }
        return true;
    }
}
