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

    [Header("Battery Charging")]
    public GameObject BatteryCable;
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

        if (isRedClampPlaced && isBlackClampPlaced && !isCharging)
        {
            StartCharging();
        }
    }

    public void StartCharging()
    {
        if (isRedClampPlaced && isBlackClampPlaced && !isCharging)
        {
            SoundManager.instance.PlaySound("BatteryCharging"); // Reproduce el sonido de carga de la batería
            StartCoroutine(ChargeBattery());
            BatteryCable.SetActive(true); // Activa el cable de carga
        }
        else
        {
            Debug.LogWarning("Clamps are not properly placed. Cannot start charging.");
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
        CompleteStep("D"); // Completa el paso al finalizar la carga
        isCharging = false;
    }
}
