using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Wheels : MonoBehaviour
{
    public bool isDesinflando;

    public GameObject desinflandoPrefab;
    public Transform desinflandoSpawnPoint;
    private GameObject desinflandoInstance;
    public XRGrabInteractable grabInteractable;
    public TaskStepController taskStepController;

    [Header("CloseUp")]
    public GameObject[] pernos;
    public Transform closeUpPosition;
    [SerializeField] private bool isCloseUp = false;
    private bool[] pernosQuitados;

    [SerializeField] private ActionBasedContinuousMoveProvider moveProvider;
    [SerializeField] private ActionBasedSnapTurnProvider snapTurnProvider;

    public void Start()
    {
        pernosQuitados = new bool[pernos.Length];
        moveProvider = FindObjectOfType<ActionBasedContinuousMoveProvider>();
        snapTurnProvider = FindObjectOfType<ActionBasedSnapTurnProvider>();
    }

    public void StartDesinflando()
    {
        if (desinflandoPrefab != null && desinflandoInstance == null)
        {
            desinflandoInstance = Instantiate(desinflandoPrefab, desinflandoSpawnPoint.position, Quaternion.identity, transform);
        }
    }

    public void StopDesinflando()
    {
        if (desinflandoInstance != null)
        {
            Destroy(desinflandoInstance);
            desinflandoInstance = null; // Asegurarse de que la referencia se elimine después de destruir el objeto
        }
    }

    public void TrashWheel()
    {
        taskStepController.CompleteStep("E");
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TrashCan"))
        {
            TrashWheel();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Llave"))
        {
            AddInteractionLayer(LayerMask.NameToLayer("Default"));
            taskStepController.CompleteStep("B");
        }
    }

    public void AcercarLlanta()
    {
        if (!isCloseUp)
        {
            transform.position = closeUpPosition.position;
            transform.rotation = closeUpPosition.rotation;
            isCloseUp = true;

            if (moveProvider != null)
                moveProvider.enabled = false;

            if (snapTurnProvider != null)
                snapTurnProvider.enabled = false;
        }
    }

    public void AlejarLlanta()
    {
        if (isCloseUp)
        {
            // Lógica para alejar la llanta (si es necesario)
            isCloseUp = false;

            if (moveProvider != null)
                moveProvider.enabled = true;

            if (snapTurnProvider != null)
                snapTurnProvider.enabled = true;
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

    public bool CanInteractWithWheel()
    {
        foreach (bool pernoQuitado in pernosQuitados)
        {
            if (!pernoQuitado)
                return false;
        }
        return true;
    }
}