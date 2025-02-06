using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WheelsBuenas : MonoBehaviour
{

    public XRGrabInteractable grabInteractable;
    public TaskStepController taskStepController;

    public void TrashWheel()
    {
        //taskStepController.CompleteStep("RetiraRuedas");
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Llave"))
        {
            taskStepController.CompleteStep("G");
        }
    }

    public void AddInteractionLayer(int layer)
    {
        grabInteractable.interactionLayers |= (1 << layer);
    }

    public void RemoveInteractionLayer(int layer)
    {
        grabInteractable.interactionLayers &= ~(1 << layer);
    }
}