using UnityEngine;

public class Door : Interactive
{
    enum DoorSound
    {
        OPEN,
        CLOSE,
        LOCK
    }

    [SerializeField] AudioClip[] _AudioClip;
    [SerializeField] GameObject _DoorLock;
    [SerializeField] Vector3 _OpenPosition = new Vector3(0,90,0);
    [SerializeField] Vector3 _ClosePosition = new Vector3(0,0,0);
    [SerializeField] bool _isOpen;
    [SerializeField] bool _isLock = true;

    InteractiveObjectRecorder _interactiveObjectRecorder;
    AgentActions _action;
    AudioSource _AudioSource;

    private new void Start()
    {
        base.Start();
        Setup();
    }

    void Setup()
    {
        _AudioSource = GetComponent<AudioSource>();
        _interactiveObjectRecorder = GetComponent<InteractiveObjectRecorder>();
        _interactiveObjectRecorder.SetStatus(_isOpen);
        _DoorLock.GetComponent<Renderer>().material.color = _isLock ? Color.red : Color.green;

        if(_isOpen && transform.localEulerAngles != _OpenPosition)
        {
            Open();
        }
    }

    void Open()
    {
        transform.localEulerAngles = _OpenPosition;
        PlaySound(DoorSound.OPEN);
    }

    void Close()
    {
        transform.localEulerAngles = _ClosePosition;
        PlaySound(DoorSound.CLOSE);
    }

    private void OnTriggerExit(Collider other)
    {
        _action = other.GetComponent<AgentActions>();
        if (!_action || !_action.enabled) return;
        m_HUD.HideActionPrompt();
        _action.SetInteract(false);
    }

    private void OnTriggerStay(Collider other)
    {
        _action = other.GetComponent<AgentActions>();
        if (_action && _action.enabled)
        {
            m_HUD.ShowActionPrompt(_interactiveObjectRecorder.GetStatus() ? "Close door." : "Open door.");
            _action.SetInteract(true);
            _action.SetInteractionObject(this);
        }
    }

    public override void Interact()
    {
        if(!_isLock)
        {
            _interactiveObjectRecorder.ObjectInteraction(!_interactiveObjectRecorder.GetStatus());
        }
        else
        {
            FindObjectOfType<HUD>().ShowMessages("Door is locked", 3f);
            PlaySound(DoorSound.LOCK);
        }
    }

    public override void MoveObject()
    {
        Open();
    }

    public override void ResetObject()
    {
        Close();
    }

    public void Unlock()
    {
        GetComponent<PhotonView>().RPC("RPCUnlock", PhotonTargets.All);
    }

    [PunRPC]
    void RPCUnlock()
    {
        _isLock = false;
        _DoorLock.GetComponent<Renderer>().material.color = Color.green;
    }

    void PlaySound(DoorSound soundID)
    {
        _AudioSource.clip = _AudioClip[(int)soundID];
        _AudioSource.Play();
    }
}