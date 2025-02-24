using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

/// <summary>
/// Add this to your interactable to make it snap to the source of the XR Ray Interactor
/// instead of staying at a distance. Has a similar outcome as enabling Force Grab.
/// </summary>
public class RayAttachModifier : MonoBehaviour
{
    IXRSelectInteractable m_SelectInteractable;
    private Vector3 originalScale;
    public Vector3 grabbedScale = Vector3.one; // Tamaño deseado al ser tomado

    protected void OnEnable()
    {
        m_SelectInteractable = GetComponent<IXRSelectInteractable>();
        if (m_SelectInteractable as Object == null)
        {
            Debug.LogError($"Ray Attach Modifier missing required Select Interactable on {name}", this);
            return;
        }

        m_SelectInteractable.selectEntered.AddListener(OnSelectEntered);
        m_SelectInteractable.selectExited.AddListener(OnSelectExited);
    }

    protected void OnDisable()
    {
        if (m_SelectInteractable as Object != null)
        {
            m_SelectInteractable.selectEntered.RemoveListener(OnSelectEntered);
            m_SelectInteractable.selectExited.RemoveListener(OnSelectExited);
        }
    }

    void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (!(args.interactorObject is XRRayInteractor))
            return;

        // Guardar el tamaño original
        originalScale = transform.localScale;

        // Cambiar el tamaño al ser tomado
        transform.localScale = grabbedScale;

        var attachTransform = args.interactorObject.GetAttachTransform(m_SelectInteractable);
        var originalAttachPose = args.interactorObject.GetLocalAttachPoseOnSelect(m_SelectInteractable);
        attachTransform.SetLocalPose(originalAttachPose);
    }

    void OnSelectExited(SelectExitEventArgs args)
    {
        // Regresar al tamaño original al ser soltado
        transform.localScale = originalScale;
    }
}

