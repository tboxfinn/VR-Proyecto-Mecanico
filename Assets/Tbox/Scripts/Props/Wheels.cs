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

    [Header("Pernos")]
    public GameObject[] pernos;
    private bool[] pernosQuitados;

    [Header("Animación")]
    public float pernoRotationDuration = 2.0f; // Duración de la rotación en segundos

    private CloseUpAttachModifier closeUpAttachModifier;
    private RayAttachModifier rayAttachModifier;

    public void Start()
    {
        pernosQuitados = new bool[pernos.Length];

        closeUpAttachModifier = GetComponent<CloseUpAttachModifier>();
        rayAttachModifier = GetComponent<RayAttachModifier>();
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

    public void AddInteractionLayer(int layer)
    {
        grabInteractable.interactionLayers |= (1 << layer);
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

    public void DesatornillarPerno(int index)
    {
        if (index >= 0 && index < pernosQuitados.Length)
        {
            StartCoroutine(RotatePerno(pernos[index], index));
        }
        else
        {
            Debug.LogWarning($"Índice de perno {index} fuera de rango.");
        }
    }

    private IEnumerator RotatePerno(GameObject perno, int index)
    {
        float elapsed = 0.0f;
        Quaternion initialRotation = perno.transform.localRotation;
        Quaternion targetRotation = initialRotation * Quaternion.Euler(360, 0, 0); // Rotar 360 grados en el eje X

        SoundManager.instance.PlaySound("Perno");

        while (elapsed < pernoRotationDuration)
        {
            perno.transform.localRotation = Quaternion.Slerp(initialRotation, targetRotation, elapsed / pernoRotationDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        perno.transform.localRotation = targetRotation;
        pernosQuitados[index] = true;
        Debug.Log($"Perno {index} desatornillado.");

        // Desvincular el perno de la rueda y añadir un Rigidbody para que caiga
        perno.transform.parent = null;
        Rigidbody rb = perno.AddComponent<Rigidbody>();
        rb.mass = 0.1f; // Ajusta la masa según sea necesario

        // Desactivar el isTrigger del Collider
        Collider collider = perno.GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = false;
        }

        if (CanInteractWithWheel())
        {
            Debug.Log("Todos los pernos han sido desatornillados. Saliendo del close-up.");
            closeUpAttachModifier.ExitCloseUp();
            rayAttachModifier.enabled = true;
        }
    }
}