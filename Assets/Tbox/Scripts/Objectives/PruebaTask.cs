using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruebaTask : MonoBehaviour
{
    public ObjectiveManager objectiveManager;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            objectiveManager.CompleteObjectiveByTaskName("Oil");
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            objectiveManager.CompleteObjectiveByTaskName("Wheel");
        }
    }
}
