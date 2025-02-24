using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PernoInteraction : MonoBehaviour
{
    public Wheels wheels;
    public int pernoIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Llave"))
        {
            Debug.Log($"Interacción con la llave detectada en el perno {pernoIndex}.");
            wheels.DesatornillarPerno(pernoIndex);
        }
    }
}
