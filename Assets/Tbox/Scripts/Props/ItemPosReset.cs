using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPosReset : MonoBehaviour
{
    private Vector3 initialPosition; // Store the initial position
    private Quaternion initialRotation; // Store the initial rotation
    private float timeOnFloor = 0f; // Timer to track how long the object has been on the floor
    public float resetTime = 5f; // Time in seconds before the object resets to its initial position
    private bool isOnFloor = false; // Flag to check if the object is on the floor
    public LayerMask floorLayer; // Layer mask to check if the object is on the floor

    void Start()
    {
        // Store the initial position and rotation of the object
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    void Update()
    {
        if (isOnFloor)
        {
            // Increment the timer
            timeOnFloor += Time.deltaTime;

            // Check if the object has been on the floor for too long
            if (timeOnFloor >= resetTime)
            {
                // Reset the object's position and rotation to the initial values
                transform.position = initialPosition;
                transform.rotation = initialRotation;
                timeOnFloor = 0f; // Reset the timer
            }
        }
        else
        {
            // Reset the timer if the object is not on the floor
            timeOnFloor = 0f;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the object collided with the floor
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isOnFloor = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // Check if the object stopped colliding with the floor
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isOnFloor = false;
        }
    }
}
