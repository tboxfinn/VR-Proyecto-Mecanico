using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskWheels : TaskStepController
{
    [Header("Wheels")]
    public Wheels[] wheels;
    public int minWheelsDes = 1;
    public int maxWheelsDes = 3;

    public override void Start()
    {
        base.Start();
        DesinflarWheels();
    }

    // Update is called once per frame
    public override void Update()
    {
        
    }

    private void DesinflarWheels()
    {
        maxWheelsDes = Mathf.Min(maxWheelsDes, wheels.Length);
        
        int numWheelsDes = Random.Range(minWheelsDes, maxWheelsDes + 1);

        // Convert the array to a list and shuffle it
        List<Wheels> shuffledWheels = new List<Wheels>(wheels);
        ShuffleList(shuffledWheels);

        // Activate the method on the selected number of wheels
        for (int i = 0; i < numWheelsDes; i++)
        {
            shuffledWheels[i].StartDesinflando();
        }
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
