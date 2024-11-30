using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gato : MonoBehaviour
{
    public GameObject originalGatoModel;
    public GameObject newGatoModel; // Nuevo modelo del gato

    public void Start()
    {
        if (newGatoModel != null)
        {
            newGatoModel.SetActive(false);
            originalGatoModel.SetActive(true);
        }
    }

    public void ChangeObject()
    {
        if (newGatoModel != null)
        {
            newGatoModel.SetActive(true);
            originalGatoModel.SetActive(false);
        }
    } 

    public void ResetObject()
    {
        if (newGatoModel != null)
        {
            newGatoModel.SetActive(false);
            originalGatoModel.SetActive(true);
        }
    }


}
