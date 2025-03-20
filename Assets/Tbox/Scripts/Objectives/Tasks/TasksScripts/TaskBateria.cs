using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskBateria : TaskStepController
{
    public GameObject redClamp;
    public GameObject blackClamp;
    public Transform redClampTarget;
    public Transform blackClampTarget;
    public float placementTolerance = 0.05f; // Tolerancia para considerar que la pinza está en el lugar correcto

    private bool isRedClampPlaced = false;
    private bool isBlackClampPlaced = false;

    [Header("Battery Charging")]
    public GameObject BatteryCable;
    public GameObject[] ClampVisuals;
    public float chargingTime = 5f; // Tiempo de carga en segundos, configurable desde el inspector
    private bool isCharging = false;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        CheckClampsPlacement();
    }

    private void CheckClampsPlacement()
    {
        // Verifica si la pinza roja está en su lugar
        if (Vector3.Distance(redClamp.transform.position, redClampTarget.position) <= placementTolerance)
        {
            if (!isRedClampPlaced)
            {
                isRedClampPlaced = true;
                Debug.Log("Red clamp placed correctly.");
            }
        }
        else
        {
            if (isRedClampPlaced)
            {
                isRedClampPlaced = false;
                Debug.Log("Red clamp removed.");
            }
        }

        // Verifica si la pinza negra está en su lugar
        if (Vector3.Distance(blackClamp.transform.position, blackClampTarget.position) <= placementTolerance)
        {
            if (!isBlackClampPlaced)
            {
                isBlackClampPlaced = true;
                Debug.Log("Black clamp placed correctly.");
            }
        }
        else
        {
            if (isBlackClampPlaced)
            {
                isBlackClampPlaced = false;
                Debug.Log("Black clamp removed.");
            }
        }

        // Completa el Step D si ambas pinzas están colocadas correctamente
        if (isRedClampPlaced && isBlackClampPlaced && GetCurrentStep() == "D")
        {
            CompleteStep("D");
            Debug.Log("Step D completed: Both clamps are placed correctly.");
        }

        // Solo comienza a cargar si ambas pinzas están colocadas al mismo tiempo
        if (isRedClampPlaced && isBlackClampPlaced && !isCharging)
        {
            StartCharging();
        }
        else if (isCharging && (!isRedClampPlaced || !isBlackClampPlaced))
        {
            StopCharging();
        }
    }

    public void StartCharging()
    {
        // Verifica si la tarea está activa y si es el paso correcto
        if (targetTask != null && targetTask.isVisible && GetCurrentStep() == "E")
        {
            if (isRedClampPlaced && isBlackClampPlaced && !isCharging)
            {
                SoundManager.instance.PlaySound("BatteryCharging"); // Reproduce el sonido de carga de la batería
                StartCoroutine(ChargeBattery());
                BatteryCable.SetActive(true); // Activa el cable de carga

                // Desactiva los ClampVisuals mientras carga
                SetClampVisualsActive(false);
            }
            else
            {
                Debug.LogWarning("Clamps are not properly placed. Cannot start charging.");
            }
        }
        else
        {
            Debug.LogWarning("Cannot charge the battery. Either the task is not active or it's not the correct step.");
        }
    }

    private void StopCharging()
    {
        if (isCharging)
        {
            Debug.Log("Charging stopped because a clamp was removed.");
            StopAllCoroutines(); // Detiene la corrutina de carga
            SoundManager.instance.PauseSound("BatteryCharging"); // Pausa el sonido de carga
            BatteryCable.SetActive(false); // Desactiva el cable de carga
            isCharging = false;

            // Reactiva los ClampVisuals si la carga se detiene
            SetClampVisualsActive(true);
        }
    }

    private IEnumerator ChargeBattery()
    {
        isCharging = true;
        Debug.Log("Battery charging started...");
        yield return new WaitForSeconds(chargingTime);
        Debug.Log("Battery charging completed!");
        SoundManager.instance.PauseSound("BatteryCharging"); // Pausa el sonido de carga de la batería
        SoundManager.instance.PlaySound("BatteryCharged"); // Reproduce el sonido de carga completada
        CompleteStep("E"); // Completa el paso E al finalizar la carga
        Debug.Log("Step E completed: Battery charging finished.");
        isCharging = false;

        // Reactiva los ClampVisuals después de cargar
        SetClampVisualsActive(true);

        // Desactiva el cable de carga al finalizar
        BatteryCable.SetActive(false);
    }

    private void SetClampVisualsActive(bool isActive)
    {
        foreach (var clampVisual in ClampVisuals)
        {
            if (clampVisual != null)
            {
                clampVisual.SetActive(isActive);
            }
        }
    }
}
