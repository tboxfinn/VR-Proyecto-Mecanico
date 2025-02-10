using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookTurnManager : MonoBehaviour
{
    public bool isBookOpen = false;
    public enum PageEnum
    {
        Left,
        Right
    }

    public Collider[] pageColliders;
    public LayerMask pageTouchPadLayerMask;
    

    public void Awake()
    {
        foreach (var collider in pageColliders)
        {
            if (collider.GetComponent<Rigidbody>() == null)
            {
                Rigidbody rb = collider.gameObject.AddComponent<Rigidbody>();
                rb.isKinematic = true;
            }
        }
    }

    public virtual void Toggle(PageEnum page, bool on)
    {
        pageColliders[(int)page].gameObject.SetActive(on);
    }

    public void TurnPageLeft()
    {
        // Lógica para girar la página a la izquierda
        Debug.Log("Turning page to the left");
    }

    public void TurnPageRight()
    {
        // Lógica para girar la página a la derecha
        Debug.Log("Turning page to the right");
    }
}
