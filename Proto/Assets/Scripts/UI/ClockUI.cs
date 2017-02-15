using System;
using UnityEngine;

public class ClockUI : MonoBehaviour
{
    private Transform _seconds;
    private Transform _minutes;
    private Transform _arrow;
    [SerializeField] private int _speed = 10;

    private void Start()
    {
        foreach (Transform child in transform)
        {
            if (_seconds != null && _minutes != null) break;
            if (child.name.IndexOf("pivotSeconds", StringComparison.InvariantCultureIgnoreCase) > -1) _seconds = child;
            else if (child.name.IndexOf("pivotMinutes", StringComparison.InvariantCultureIgnoreCase) > -1) _minutes = child;
            else if (child.name.IndexOf("pivotArrow", StringComparison.InvariantCultureIgnoreCase) > -1) _arrow = child;
        }
    }

    private void Update()
    {
        RotateLikeAClock(75);
    }

    private bool a = true;
    private void RotateLikeAClock(int time)
    {
        if(a)_seconds.Rotate(0f, 0f, GetSecondClockPosition(time));
        if(a)_minutes.Rotate(0f, 0f, GetMinuteClockPosition(time));
        if(a)_arrow.Rotate(0f, 0f, GetSecondClockPosition(time));
        a = false;
    }

    private float GetSecondClockPosition(int time)
    {
        var seconds = time % 60;
        return -(360 / 60 * seconds + 2);
    }

    private float GetMinuteClockPosition(int time)
    {
        var seconds = time / 60;
        return -(360 / 12 * seconds + 2);
    }
}