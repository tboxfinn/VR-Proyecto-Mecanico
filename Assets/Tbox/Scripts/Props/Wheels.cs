using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheels : MonoBehaviour
{
    public bool isDesinflando;

    public GameObject desinflandoPrefab;
    public Transform desinflandoSpawnPoint;
    private GameObject desinflandoInstance;

    public void StartDesinflando()
    {
        if (desinflandoPrefab != null && desinflandoInstance == null)
        {
            desinflandoInstance = Instantiate(desinflandoPrefab, desinflandoSpawnPoint.position, Quaternion.identity, transform);
        }
    }

    public void StopDesinflando()
    {
        if (desinflandoInstance != null)
        {
            Destroy(desinflandoInstance);
            desinflandoInstance = null; // Asegurarse de que la referencia se elimine después de destruir el objeto
        }
    }

    public void TrashWheel()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TrashCan"))
        {
            TrashWheel();
        }
    }
}