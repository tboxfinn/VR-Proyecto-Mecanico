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

    [Header("Guide")]
    public GameObject guidePrefab;
    private GameObject currentGuidePopUp;

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

        Debug.Log($"Showing popup for: {targetObject.name}");

        currentPopUp.SetActive(true);
    }

    public void HidePopUp()
    {
        if (currentPopUp != null)
        {
            Destroy(currentPopUp);
        }
    }

    public void ShowGuidePopUp(string guideText)
    {
        if (guidePrefab == null)
        {
            Debug.LogWarning("Guide popUpPrefab is null");
            return;
        }

        // Destroy any existing guide popup
        if (currentGuidePopUp != null)
        {
            Destroy(currentGuidePopUp);
        }

        // Instantiate a new guide popup as a child of the PopUpManager
        currentGuidePopUp = Instantiate(guidePrefab, transform);
        currentGuidePopUp.transform.SetParent(Camera.main.transform, false); // Attach to the camera

        // Get the TextMeshProUGUI component from the children of the guide popup
        var guideTextComponent = currentGuidePopUp.transform.Find("Panel/GuideText").GetComponent<TextMeshProUGUI>();

        if (guideTextComponent == null)
        {
            Debug.LogError("GuideText component not found in the guide popup prefab");
            return;
        }

        // Set the guide text value
        guideTextComponent.text = guideText;

        Debug.Log("Showing guide popup");

        currentGuidePopUp.SetActive(true);
    }

    public void HideGuidePopUp()
    {
        if (currentGuidePopUp != null)
        {
            Destroy(currentGuidePopUp);
        }
    }
}