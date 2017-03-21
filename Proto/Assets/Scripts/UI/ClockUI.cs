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
    private static float _minuteToRewind;

    private static int _totalTime;

    private void OnEnable()
    {
        InputMode.isInMenu = true;
        _photonView = GetComponent<PhotonView>();
        if(!_isFirst)_photonView.RPC("RPCStopTime", PhotonTargets.All);
        _timeController = FindObjectOfType<TimeController>();
        _mainRecorder = FindObjectOfType<MainRecorder>();
        _textTimeRewind = GetComponentInChildren<Text>();
        var clockTransform = transform.FindChild("Clock").transform;
        _seconds = clockTransform.FindChild("PivotSeconds");
        _arrow = clockTransform.FindChild("PivotArrow");
        _curTime = _prevTime = _timeController.Time;
        _totalTime = _timeController.MaxTime;
        Debug.Log(_totalTime);
        UpdateClock(_curTime);
        StartCoroutine(ReadInput());
    }

    private void OnDisable()
    {
        if(!_isFirst && PhotonNetwork.connected)_photonView.RPC("RPCStartTime", PhotonTargets.All);
        _isFirst = false;
        InputMode.isInMenu = false;
    }

    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    private IEnumerator ReadInput()
    {
        if (Input.GetButton("Action"))
        {
            _prevTime = _curTime;
            ExecuteTimeRewind();
            Toggle();
        }
        _curTime = TuneSeconds(-Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), (int)_curTime);
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

    private static float TuneSeconds(float x, float y, int currentTime)
    {
        if (x == 0 && y == 0) return currentTime;
        var time = Mathf.Atan2(x, y) * Mathf.Rad2Deg / 360 * _totalTime;
        return time < 0 ? _totalTime + time : time;
    }

    private void UpdateClock(float time)
    {
        var timeRewindedTo = _timeController.MaxTime - time;
        _textTimeRewind.text = string.Format("{0}:{1:00}", (int)timeRewindedTo / 60, (int)timeRewindedTo % 60);
        _arrow.rotation = Quaternion.Euler(0f, 0f, GetSecondClockPosition(timeRewindedTo));
        _seconds.rotation = Quaternion.Euler(0f, 0f, GetSecondClockPosition(timeRewindedTo));
    }

    private static float GetSecondClockPosition(float time)
    {
        var seconds = time % _totalTime;
        return -(360/_totalTime * seconds + 2);
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