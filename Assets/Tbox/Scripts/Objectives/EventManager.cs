using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public event Action<string> OnObjectiveCompleted;

    public void ObjectiveCompleted(string objectiveID)
    {
        OnObjectiveCompleted?.Invoke(objectiveID);
    }
}
