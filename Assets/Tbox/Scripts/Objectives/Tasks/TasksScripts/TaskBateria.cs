using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskBateria : TaskStepController
{
    public GameObject redClamp;
    public GameObject blackClamp;
    public Transform redClampTarget;
    public Transform blackClampTarget;
    public float placementTolerance = 0.1f; // Tolerancia para considerar que la pinza está en el lugar correcto
    

    private bool isRedClampPlaced = false;
    private bool isBlackClampPlaced = false;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
        CheckClampsPlacement();
    }

    private void CheckClampsPlacement()
    {
        if (!isRedClampPlaced && Vector3.Distance(redClamp.transform.position, redClampTarget.position) <= placementTolerance)
        {
            isRedClampPlaced = true;
            Debug.Log("Red clamp placed correctly.");
        }

        if (!isBlackClampPlaced && Vector3.Distance(blackClamp.transform.position, blackClampTarget.position) <= placementTolerance)
        {
            isBlackClampPlaced = true;
            Debug.Log("Black clamp placed correctly.");
        }

        if (isRedClampPlaced && isBlackClampPlaced)
        {
            CompleteTask();
        }
    }

    private void CompleteTask()
    {
        Debug.Log("Both clamps placed correctly. Task completed.");
        CompleteStep("Bateria");
    }
}
