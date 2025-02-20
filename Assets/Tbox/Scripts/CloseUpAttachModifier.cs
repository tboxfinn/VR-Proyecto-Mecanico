using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;

namespace UnityEngine.XR.Content.Interaction
{
    /// <summary>
    /// Add this to your interactable to make it snap to the close-up position instead of staying at a distance.
    /// </summary>
    public class CloseUpAttachModifier : MonoBehaviour
    {
        IXRSelectInteractable m_SelectInteractable;
        public Transform closeUpPosition; // Referencia a la posición de close-up
        private XRGrabInteractable grabInteractable;
        private Vector3 originalPosition;
        private Quaternion originalRotation;
        private LocomotionSystem locomotionSystem;

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

            // Guardar la posición y rotación originales
            originalPosition = transform.position;
            originalRotation = transform.rotation;

            // Mover el objeto a la posición de close-up
            if (closeUpPosition != null)
            {
                transform.position = closeUpPosition.position;
                transform.rotation = closeUpPosition.rotation;
            }

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
            // Devolver el objeto a su posición y rotación originales
            transform.position = originalPosition;
            transform.rotation = originalRotation;

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

            isCloseUp = false; // Actualizar el estado de close-up
        }
    }
}