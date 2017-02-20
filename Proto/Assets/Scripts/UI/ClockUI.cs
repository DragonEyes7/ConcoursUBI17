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
    private float curTime;

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
        curTime = 20;
        UpdateClock(curTime);
    }

    private void Update()
    {
        var angle = Mathf.Atan2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Mathf.Rad2Deg;
        curTime = angle / 360 * 60;
        curTime = curTime < 0 ? 60 + curTime : curTime;
        Debug.Log("Rotation angle : " + curTime);
        UpdateClock(curTime);
        //_arrow.rotation = Quaternion.Euler(new Vector3(0, 0, -angle));
    }

    private int UpdateClock(float time)
    {
        _textTimeRewind.text = string.Format("{0}:{1:00}", (int)time / 60, (int)time % 60);
        _arrow.rotation = Quaternion.Euler(0f, 0f, GetSecondClockPosition(time));
        _seconds.rotation = Quaternion.Euler(0f, 0f, GetSecondClockPosition(time));
        _minutes.rotation = Quaternion.Euler(0f, 0f, GetMinuteClockPosition(time));
        return 0;
    }

    private static float GetSecondClockPosition(float time)
    {
        var seconds = time % 60;
        return -(6 * seconds + 2);
    }

    private static float GetMinuteClockPosition(float time)
    {
        var seconds = time / 60;
        return -(30 * seconds + 2);
    }
}