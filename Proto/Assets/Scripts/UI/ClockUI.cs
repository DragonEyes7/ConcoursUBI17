using System;
using UnityEngine;
using UnityEngine.UI;

public class ClockUI : MonoBehaviour
{
    private Transform _seconds;
    private Transform _minutes;
    private Transform _arrow;
    private Text _textTimeRewind;
    [SerializeField] private int _speed = 10;
    private TimeController _timeController;

    private void Start()
    {
        _timeController = FindObjectOfType<TimeController>();
        _textTimeRewind = GetComponentInChildren<Text>();
        foreach (Transform child in transform)
        {
            if (_seconds != null && _minutes != null) break;
            if (child.name.IndexOf("pivotSeconds", StringComparison.InvariantCultureIgnoreCase) > -1) _seconds = child;
            else if (child.name.IndexOf("pivotMinutes", StringComparison.InvariantCultureIgnoreCase) > -1) _minutes = child;
            else if (child.name.IndexOf("pivotArrow", StringComparison.InvariantCultureIgnoreCase) > -1) _arrow = child;
        }
        _timeController.Tick.Suscribe(UpdateClock);
        UpdateClock(0);
    }

    private int UpdateClock(int time)
    {
        _textTimeRewind.text = string.Format("{0}:{1:00}", time / 60, time % 60);
        _arrow.rotation = Quaternion.Euler(0f, 0f, GetSecondClockPosition(time));
        _seconds.rotation = Quaternion.Euler(0f, 0f, GetSecondClockPosition(time));
        _minutes.rotation = Quaternion.Euler(0f, 0f, GetMinuteClockPosition(time));
        return 0;
    }

    private static float GetSecondClockPosition(int time)
    {
        var seconds = time % 60;
        return -(360 / 60 * seconds + 2);
    }

    private static float GetMinuteClockPosition(int time)
    {
        var seconds = time / 60;
        return -(360 / 12 * seconds + 2);
    }
}