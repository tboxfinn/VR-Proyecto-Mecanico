using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilTask : MonoBehaviour
{
    // Start is called before the first frame update
    public TaskObjectiveSO taskToComplete;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GrabOilObjective()
    {
        CompleteObjective();
    }

    private void CompleteObjective()
    {
        if (taskToComplete != null)
        {
            if (!taskToComplete.IsTaskCompleted())
            {
                taskToComplete.CompleteCurrentObjective();
            }
        }
    }

    
}
