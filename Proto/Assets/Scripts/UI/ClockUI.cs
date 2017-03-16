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
    private MainRecorder _mainRecorder;
    private float _curTime, _prevTime;
    private PhotonView _photonView;
    private bool _isFirst = true;

    private void OnEnable()
    {
        _photonView = GetComponent<PhotonView>();
        if(!_isFirst)_photonView.RPC("RPCStopTime", PhotonTargets.All);
        _timeController = FindObjectOfType<TimeController>();
        _mainRecorder = FindObjectOfType<MainRecorder>();
        _textTimeRewind = GetComponentInChildren<Text>();
        foreach (Transform child in transform)
        {
            if (_seconds != null && _minutes != null) break;
            if (child.name.IndexOf("pivotSeconds", StringComparison.InvariantCultureIgnoreCase) > -1) _seconds = child;
            else if (child.name.IndexOf("pivotMinutes", StringComparison.InvariantCultureIgnoreCase) > -1) _minutes = child;
            else if (child.name.IndexOf("pivotArrow", StringComparison.InvariantCultureIgnoreCase) > -1) _arrow = child;
        }
        _curTime = _prevTime = _timeController.time;
        UpdateClock(_curTime);
        StartCoroutine(ReadInput());
    }

    private void OnDisable()
    {
        if(!_isFirst && PhotonNetwork.connected)_photonView.RPC("RPCStartTime", PhotonTargets.All);
        _isFirst = false;
    }

    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    private IEnumerator ReadInput()
    {
        if (Input.GetButtonDown("Action"))
        {
            _prevTime = _curTime;
            ExecuteTimeRewind();
            Toggle();
        }
        _curTime = TuneMinutes(-Input.GetAxisRaw("DPadY"), (int)_curTime/60) +
                   TuneSeconds(-Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), (int)_curTime % 60);
        if (_curTime > _prevTime) _curTime = _prevTime;
        if (_curTime < 0) _curTime = 0;
        UpdateClock(_curTime);
        yield return new WaitForSecondsRealtime(0.01f);
        StartCoroutine(ReadInput());
    }

    private void ExecuteTimeRewind()
    {
        _mainRecorder.DoRewind((int)_curTime);
    }

    private static float TuneMinutes(float y, int currentMinutes)
    {
        var minutes = (currentMinutes + y) * 60;
        return minutes < 0 ? 0 : minutes;
    }

    private static float TuneSeconds(float x, float y, int prevSeconds)
    {
        if (x == 0 && y == 0) return prevSeconds;
        var time = Mathf.Atan2(x, y) * Mathf.Rad2Deg / 360 * 60;
        return time < 0 ? 60 + time : time;
    }

    private void UpdateClock(float time)
    {
        var timeRewindedTo = _timeController.maxTime - time;
        _textTimeRewind.text = string.Format("{0}:{1:00}", (int)timeRewindedTo / 60, (int)timeRewindedTo % 60);
        _arrow.rotation = Quaternion.Euler(0f, 0f, GetSecondClockPosition(timeRewindedTo));
        _seconds.rotation = Quaternion.Euler(0f, 0f, GetSecondClockPosition(timeRewindedTo));
        _minutes.rotation = Quaternion.Euler(0f, 0f, GetMinuteClockPosition(timeRewindedTo));
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

    [PunRPC]
    private void RPCStopTime()
    {
        TimeStopper.StopTime();
    }

    [PunRPC]
    private void RPCStartTime()
    {
        TimeStopper.StartTime();
    }
}