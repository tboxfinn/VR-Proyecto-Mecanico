using UnityEngine;
using TMPro;

public class ObjectiveUI : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;

    public void SetTitle(string title)
    {
        titleText.text = title;
    }

    public void SetDescription(string description)
    {
        descriptionText.text = description;
    }
}