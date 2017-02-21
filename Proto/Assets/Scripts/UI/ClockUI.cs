using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ClockUI : MonoBehaviour
{
    private Transform _seconds;
    private Transform _minutes;
    private Transform _arrow;
    private Text _textTimeRewind;
    private TimeController _timeController;
    private float _curTime;

    private void OnEnable()
    {
        TimeStopper.StopTime();
        _timeController = FindObjectOfType<TimeController>();
        _textTimeRewind = GetComponentInChildren<Text>();
        foreach (Transform child in transform)
        {
            if (_seconds != null && _minutes != null) break;
            if (child.name.IndexOf("pivotSeconds", StringComparison.InvariantCultureIgnoreCase) > -1) _seconds = child;
            else if (child.name.IndexOf("pivotMinutes", StringComparison.InvariantCultureIgnoreCase) > -1) _minutes = child;
            else if (child.name.IndexOf("pivotArrow", StringComparison.InvariantCultureIgnoreCase) > -1) _arrow = child;
        }
        Debug.Log("Time Controller time : " + _timeController.time);
        UpdateClock(_timeController.time);
        StartCoroutine(ReadInput());
    }

    private void OnDisable()
    {
        TimeStopper.StartTime();
    }

    private IEnumerator ReadInput()
    {
        //This calculates the angle the joystick is in (instead of simple x, y coords)
        var angle = Mathf.Atan2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Mathf.Rad2Deg;
        _curTime = angle / 360 * 60;
        _curTime = _curTime < 0 ? 60 + _curTime : _curTime;
        UpdateClock(_curTime);
        //_arrow.rotation = Quaternion.Euler(new Vector3(0, 0, -angle));
        yield return new WaitForSecondsRealtime(0.01f);
        StartCoroutine(ReadInput());
    }

    private void UpdateClock(float time)
    {
        _textTimeRewind.text = string.Format("{0}:{1:00}", (int)time / 60, (int)time % 60);
        _arrow.rotation = Quaternion.Euler(0f, 0f, GetSecondClockPosition(time));
        _seconds.rotation = Quaternion.Euler(0f, 0f, GetSecondClockPosition(time));
        _minutes.rotation = Quaternion.Euler(0f, 0f, GetMinuteClockPosition(time));
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