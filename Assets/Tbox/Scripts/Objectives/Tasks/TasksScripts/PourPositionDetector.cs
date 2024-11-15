using UnityEngine;

public class PourPositionDetector : MonoBehaviour
{
    [Tooltip("Tag del objeto que quieres detectar.")]
    public string targetTag = "Oil"; // Cambia esto por el tag que quieras detectar
    public string taskName;
    public TaskStepController taskToComplete;

    // Este método se llama automáticamente cuando otro objeto con un Collider entra en contacto con este Trigger
    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que entra en contacto tiene el tag especificado
        if (other.CompareTag(targetTag))
        {   
            if (taskToComplete != null)
            {
                taskToComplete.CompleteStep(taskName);
            }
        }
    }
}
