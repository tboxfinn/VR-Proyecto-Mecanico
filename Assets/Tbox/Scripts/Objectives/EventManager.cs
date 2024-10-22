using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    public delegate void ObjectiveEvent();
    public static event ObjectiveEvent OnObjectiveCompleted;

    public static void ObjectiveCompleted()
    {
        OnObjectiveCompleted?.Invoke();
    }
}
