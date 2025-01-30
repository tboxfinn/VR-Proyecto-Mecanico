using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpManager : MonoBehaviour
{
    public GameObject popUpPrefab;
    private GameObject currentPopUp;

    public void ShowPopUp(GameObject targetObject)
    {
        if (targetObject == null || popUpPrefab == null)
        {
            Debug.LogWarning("Target object or popUpPrefab is null");
            return;
        }

        var description = targetObject.GetComponent<ObjectDescription>();
        if (description != null)
        {
            // Destroy any existing popup
            if (currentPopUp != null)
            {
                Destroy(currentPopUp);
            }

            // Instantiate a new popup as a child of the PopUpManager
            currentPopUp = Instantiate(popUpPrefab, targetObject.transform.position + Vector3.up * 0.5f, Quaternion.identity, transform);
            currentPopUp.transform.LookAt(Camera.main.transform); // Make the popup face the camera

            // Get the TextMeshProUGUI components from the children of the popup
            var nameText = currentPopUp.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
            var descriptionText = currentPopUp.transform.Find("DescriptionText").GetComponent<TextMeshProUGUI>();

            if (nameText == null || descriptionText == null)
            {
                Debug.LogError("NameText or DescriptionText component not found in the popup prefab");
                return;
            }

            // Set the text values
            nameText.text = targetObject.name;
            descriptionText.text = description.GetDescription();

            Debug.Log($"Showing popup for: {targetObject.name} with description: {description.GetDescription()}");

            currentPopUp.SetActive(true);
        }
        else
        {
            Debug.LogWarning("ObjectDescription component not found on the target object");
        }
    }

    public void HidePopUp()
    {
        if (currentPopUp != null)
        {
            Destroy(currentPopUp);
        }
    }
}