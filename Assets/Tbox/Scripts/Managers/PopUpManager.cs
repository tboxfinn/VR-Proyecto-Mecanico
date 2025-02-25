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

    [Header("Advice")]
    public GameObject adviceHolder;
    public TMP_Text adviceText;
    public float adviceDuration = 5f;
    public float fadeInDuration = 0.2f;
    public float fadeOutDuration = 0.5f;

    public void Start()
    {
        adviceHolder.SetActive(false);
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

    public void ShowAdvice(string message)
    {
        StopAllCoroutines(); // Stop any existing coroutines to prevent overlapping
        StartCoroutine(ShowAdviceCoroutine(message, adviceDuration));
    }

    private IEnumerator ShowAdviceCoroutine(string message, float duration)
    {
        adviceText.text = message;
        yield return StartCoroutine(FadeInAdviceHolder());

        yield return new WaitForSeconds(duration);

        StartCoroutine(FadeOutAdviceHolder());
    }

    private IEnumerator FadeInAdviceHolder()
    {
        CanvasGroup canvasGroup = adviceHolder.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = adviceHolder.AddComponent<CanvasGroup>();
        }

        adviceHolder.SetActive(true);
        float startAlpha = canvasGroup.alpha;
        float rate = 1.0f / fadeInDuration;
        float progress = 0.0f;

        while (progress < 1.0f)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 1, progress);
            progress += rate * Time.deltaTime;

            yield return null;
        }

        canvasGroup.alpha = 1;
    }

    private IEnumerator FadeOutAdviceHolder()
    {
        CanvasGroup canvasGroup = adviceHolder.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = adviceHolder.AddComponent<CanvasGroup>();
        }

        float startAlpha = canvasGroup.alpha;
        float rate = 1.0f / fadeOutDuration;
        float progress = 0.0f;

        while (progress < 1.0f)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, progress);
            progress += rate * Time.deltaTime;

            yield return null;
        }

        canvasGroup.alpha = 0;
        adviceHolder.SetActive(false);
    }

}