using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hood : MonoBehaviour
{
    public bool isOpen = false; // Variable de estado para rastrear si el capó está abierto o cerrado
    private float closedRotationX = 90f; // Rotación en X cuando el capó está cerrado
    public AnimationCurve rotationCurve; // Curva de animación para controlar la velocidad de la rotación
    public float duration = 1f; // Duración de la rotación
    public CarGato carGato; // Referencia al script CarGato

    public void OpenCloseHood(float openRotationX)
    {
        StopAllCoroutines(); // Detener cualquier corrutina en ejecución
        if (isOpen)
        {
            // Si el capó está abierto, ciérralo
            StartCoroutine(RotateHood(closedRotationX));
        }
        else
        {
            // Si el capó está cerrado, ábrelo
            StartCoroutine(RotateHood(openRotationX));
        }

        // Alternar el estado del capó
        isOpen = !isOpen;
    }

    private IEnumerator RotateHood(float targetRotationX)
    {
        if (carGato.carLevantado)
        {
            // Si el coche está levantado, no se puede abrir el capó
            Debug.Log("No se puede abrir el capó con el coche levantado");
            yield break;
        }

        float elapsedTime = 0f;
        Quaternion initialRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(targetRotationX, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            float curveValue = rotationCurve.Evaluate(t);
            transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, curveValue);
            yield return null;
        }

        // Asegurarse de que la rotación final sea exactamente la deseada
        transform.rotation = targetRotation;
    }
}
