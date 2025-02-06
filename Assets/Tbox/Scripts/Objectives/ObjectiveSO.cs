using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewObjective", menuName = "Objective System/Objective")]
public class ObjectiveSO : ScriptableObject
{
    [TextArea(3, 10)]
    public string description;
    public bool isCompleted;
    public List<GameObject> objectsToOutline;

    public virtual void CompleteObjective()
    {
        isCompleted = true;
    }
    
}
