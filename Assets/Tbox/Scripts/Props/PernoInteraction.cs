using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PernoInteraction : MonoBehaviour
{
    public Wheels wheels;
    public int pernoIndex;
    public float pernoRotationDuration = 2.0f; // Duración de la rotación en segundos

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Llave"))
        {
            Debug.Log($"Interacción con la llave detectada en el perno {pernoIndex}.");
            StartCoroutine(RotatePerno());
        }
    }

    private IEnumerator RotatePerno()
    {
        Transform perno = transform;
        float elapsed = 0.0f;
        Quaternion initialRotation = perno.rotation;
        Quaternion targetRotation = initialRotation * Quaternion.Euler(360, 0, 0); // Rotar 360 grados en el eje X

        SoundManager.instance.PlaySound("Perno");

        while (elapsed < pernoRotationDuration)
        {
            perno.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsed / pernoRotationDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        perno.rotation = targetRotation;
        wheels.DesatornillarPerno(pernoIndex);
    }
}
