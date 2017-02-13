using System;
using UnityEngine;

public class ClockUI : MonoBehaviour
{
    private Transform minutes;
    private Transform hours;

    private void Start()
    {
        foreach (Transform child in transform)
        {
            if (minutes != null && hours != null) break;
            if (child.name.IndexOf("minutes", StringComparison.InvariantCultureIgnoreCase) > -1) minutes = child;
            else if (child.name.IndexOf("hours", StringComparison.InvariantCultureIgnoreCase) > -1) hours = child;
        }
    }
}