using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILookAt : MonoBehaviour
{
    void LateUpdate()
    {
        // Asegúrate de que la cámara esté asignada y activa
        if (Camera.main != null)
        {
            // Mira hacia la cámara
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        }
    }
}
