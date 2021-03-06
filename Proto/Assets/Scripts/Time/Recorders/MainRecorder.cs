﻿using UnityEngine;

public class MainRecorder : MonoBehaviour
{
    [SerializeField] private TimeController _timeController;
    [SerializeField] private GameObject _rewindPrompt;
    [SerializeField] private float _rewindPromptTime = 30f;
    private int _time;
    private bool _isRecording = true;
    private PhotonView _photonView;
    private bool _hasRewinded;

    public MultipleDelegate OnTick = new MultipleDelegate();
    public MultipleDelegate OnRewind = new MultipleDelegate();

    AudioSource _AudioSource;

    public bool IsRecording
    {
        get { return _isRecording; }
    }

    public void SetTimeController(TimeController timecontroller)
    {
        _timeController = timecontroller;
    }

    void Start()
    {
        _AudioSource = GetComponent<AudioSource>();
        _timeController = _timeController ?? FindObjectOfType<TimeController>();
        _photonView = GetComponent<PhotonView>();
        _timeController.Tick.Suscribe(DoOnTick);
        _hasRewinded = false;
        HideRewindPrompt();
    }

    private int DoOnTick(int time)
    {
        if (_isRecording)
        {
            OnTick.Execute(time);
        }
        _time = time;
        if (PhotonNetwork.isMasterClient && _timeController.MaxTime - time <= _rewindPromptTime && !_hasRewinded)
        {
            ShowRewindPrompt();
        }
        else
        {
            HideRewindPrompt();
        }
        return 0;
    }

    private void ShowRewindPrompt()
    {
        if (_rewindPrompt)
            _rewindPrompt.SetActive(true);
    }

    private void HideRewindPrompt()
    {
        if (_rewindPrompt)
            _rewindPrompt.SetActive(false);
    }

    private void SetTimeForward()
    {
        _timeController.IsPlaying = true;
        _isRecording = true;
    }

    public void DoRewind(int newTime)
    {
        _AudioSource.Play();
        _isRecording = false;
        _timeController.IsPlaying = false;
        _time = newTime;
        _photonView.RPC("SetTime", PhotonTargets.All, _time);
        OnRewind.Execute(_time);
        SetTimeForward();
        _hasRewinded = true;
    }

    [PunRPC]
    private void SetTime(int time)
    {
        _timeController.Time = time;
    }
}