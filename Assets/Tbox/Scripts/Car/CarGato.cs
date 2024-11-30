using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarGato : MonoBehaviour
{
    private Transform carTransform;
    public float rotationAngle = 90f; // Ángulo de rotación en Z
    public float liftAmount = 1f; // Cantidad de elevación en Y
    private Vector3 originalPosition; // Posición original del objeto
    public bool carLevantado = false;
    public Hood hood; // Referencia al script Hood

    // Start is called before the first frame update
    void Start()
    {
        carTransform = transform; // Guarda el Transform del objeto
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RotateAndLift()
    {
        if (hood.isOpen)
        {
            // Si el capó está abierto, no se puede levantar el coche
            Debug.Log("No se puede levantar el coche con el capó abierto");
            return;
        }
        
        // Rota en el eje Z
        carTransform.Rotate(0, 0, rotationAngle);

        // Levanta en el eje Y
        carTransform.position += new Vector3(0, liftAmount, 0);

        carLevantado = true;
    }

    public void ResetPosition()
    {
        carTransform.rotation = Quaternion.identity; // Resetea la rotación
        carTransform.position = originalPosition; // Resetea la posición

        carLevantado = false;
    }
}
