using UnityEngine;

public class MainRecorder : MonoBehaviour
{
    [SerializeField] private TimeController _timeController;
    [SerializeField] private GameObject _rewindPrompt;
    private int _time;
    private bool _isRecording = true;
    private PhotonView _photonView;
    private bool _hasRewinded;

    public MultipleDelegate OnTick = new MultipleDelegate();
    public MultipleDelegate OnRewind = new MultipleDelegate();

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
        _timeController = _timeController ?? FindObjectOfType<TimeController>();
        _photonView = GetComponent<PhotonView>();
        _timeController.Tick.Suscribe(DoOnTick);
        _hasRewinded = false;
        HideRewindPrompt();
        DoOnTick(0);
    }

    void Update()
    {
        if (Input.GetButtonDown("TimeRewind"))
        {
            SetTimeRewinding();
            SetTimeForward();
            _hasRewinded = true;
        }
    }

    private int DoOnTick(int time)
    {
        if (_isRecording)
        {
            OnTick.Execute(time);
        }
        _time = time;
        if (PhotonNetwork.isMasterClient && _timeController.maxTime - time <= 10 && !_hasRewinded)
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
        _timeController.isPlaying = true;
        _isRecording = true;
    }

    private void SetTimeRewinding()
    {
        _isRecording = false;
        _timeController.isPlaying = false;
        var timeToRewind = GetMaxTime(3);
        _time -= timeToRewind;
        _photonView.RPC("SetTime", PhotonTargets.All, _time);
        OnRewind.Execute(_time);
    }

    [PunRPC]
    private void SetTime(int time)
    {
        _timeController.time = time;
    }

    private int GetMaxTime(int newTime)
    {
        return _time - newTime >= 0 ? newTime : _time;
    }
}