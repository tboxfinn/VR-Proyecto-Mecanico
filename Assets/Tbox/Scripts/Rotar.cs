using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotar : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 50f; // Velocidad de rotación en el eje Y

    [Header("Floating Settings")]
    public float floatAmplitude = 0.5f; // Amplitud del movimiento de flotación
    public float floatFrequency = 1f; // Frecuencia del movimiento de flotación

    private Vector3 startPosition;

    void Start()
    {
        // Guarda la posición inicial del objeto
        startPosition = transform.position;
    }

    void Update()
    {
        // Rotación en el eje Y global
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);

        // Movimiento de flotación (sube y baja)
        float newY = startPosition.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
