using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDescription : MonoBehaviour
{
    [TextArea]
    public string description;

    public string GetDescription()
    {
        return description;
    }
}
