using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

/// <summary>
/// Add this to your interactable to make it snap to the close-up position instead of staying at a distance.
/// </summary>
public class CloseUpAttachModifier : MonoBehaviour
{
    IXRSelectInteractable m_SelectInteractable;
    public Transform closeUpPosition; // Referencia a la posición de close-up
    public Vector3 closeUpScale = Vector3.one; // Escala del objeto en close-up
    private XRGrabInteractable grabInteractable;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 originalScale;
    private LocomotionSystem locomotionSystem;

    public GameObject[] objetosDesaparecer;

    [Header("Debug")]
    public bool isCloseUp = false;
    public bool debugExitCloseUp = false; // Booleano para depuración

    protected void OnEnable()
    {
        m_SelectInteractable = GetComponent<IXRSelectInteractable>();
        grabInteractable = GetComponent<XRGrabInteractable>();
        locomotionSystem = FindObjectOfType<LocomotionSystem>();

        if (m_SelectInteractable as Object == null)
        {
            Debug.LogError($"Close Up Attach Modifier missing required Select Interactable on {name}", this);
            return;
        }

        m_SelectInteractable.selectEntered.AddListener(OnSelectEntered);
    }

    protected void OnDisable()
    {
        if (m_SelectInteractable as Object != null)
            m_SelectInteractable.selectEntered.RemoveListener(OnSelectEntered);
    }

    void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (!(args.interactorObject is XRRayInteractor))
            return;

        // Guardar la posición, rotación y escala originales
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        originalScale = transform.localScale;
        Debug.Log($"Original Scale: {originalScale}");

        // Mover el objeto a la posición de close-up
        if (closeUpPosition != null)
        {
            transform.position = closeUpPosition.position;
            transform.rotation = closeUpPosition.rotation;
        }

        // Cambiar el tamaño al ser tomado
        transform.localScale = closeUpScale;
        Debug.Log($"Close-Up Scale: {closeUpScale}");

        // Desactivar temporalmente el XRGrabInteractable para evitar que el objeto se mueva a las manos
        if (grabInteractable != null)
        {
            grabInteractable.enabled = false;
        }

        // Desactivar el sistema de locomoción
        if (locomotionSystem != null)
        {
            locomotionSystem.enabled = false;
        }

        // Desactivar los objetos especificados
        foreach (var objeto in objetosDesaparecer)
        {
            if (objeto != null)
            {
                objeto.SetActive(false);
            }
        }

        // Ajustar la posición de adjunto
        var attachTransform = args.interactorObject.GetAttachTransform(m_SelectInteractable);
        var originalAttachPose = args.interactorObject.GetLocalAttachPoseOnSelect(m_SelectInteractable);
        attachTransform.SetLocalPose(originalAttachPose);

        isCloseUp = true; // Actualizar el estado de close-up
    }

    void Update()
    {
        // Verificar el booleano de depuración
        if (debugExitCloseUp)
        {
            ExitCloseUp();
            debugExitCloseUp = false; // Resetear el booleano
        }
    }

    public void ExitCloseUp()
    {
        // Devolver el objeto a su posición, rotación y escala originales
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        transform.localScale = originalScale;
        Debug.Log($"Restored Scale: {originalScale}");

        // Reactivar el XRGrabInteractable
        if (grabInteractable != null)
        {
            grabInteractable.enabled = true;
        }

        // Reactivar el sistema de locomoción
        if (locomotionSystem != null)
        {
            locomotionSystem.enabled = true;
        }

        // Reactivar los objetos especificados
        foreach (var objeto in objetosDesaparecer)
        {
            if (objeto != null)
            {
                objeto.SetActive(true);
            }
        }

        isCloseUp = false; // Actualizar el estado de close-up
    }
}
