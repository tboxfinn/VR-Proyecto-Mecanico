using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpManager : MonoBehaviour
{
    [Header("NamePopUp")]
    public GameObject popUpPrefab;
    public float popupHeightOffset = 0.2f;
    private GameObject currentPopUp;

    public void Start()
    {
        // No initialization needed for advice anymore
    }

    public void ShowPopUp(GameObject targetObject)
    {
        if (targetObject == null || popUpPrefab == null)
        {
            Debug.LogWarning("Target object or popUpPrefab is null");
            return;
        }

        // Destroy any existing popup
        if (currentPopUp != null)
        {
            Destroy(currentPopUp);
        }

        // Instantiate a new popup as a child of the PopUpManager
        Vector3 popupPosition = targetObject.transform.position + Vector3.up * popupHeightOffset;
        currentPopUp = Instantiate(popUpPrefab, popupPosition, Quaternion.identity, transform);
        currentPopUp.transform.LookAt(Camera.main.transform); // Make the popup face the camera

        // Get the TextMeshProUGUI component from the children of the popup
        var nameText = currentPopUp.transform.Find("Panel/NameText").GetComponent<TextMeshProUGUI>();

        if (nameText == null)
        {
            Debug.LogError("NameText component not found in the popup prefab");
            return;
        }

        // Set the text value
        nameText.text = targetObject.name;

        currentPopUp.SetActive(true);
    }

    public void HidePopUp()
    {
        if (currentPopUp != null)
        {
            Destroy(currentPopUp);
        }
    }
}