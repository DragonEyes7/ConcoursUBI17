using UnityEngine;

public class Door : Interactive
{
    private InteractiveObjectRecorder _interactiveObjectRecorder;
    [SerializeField] GameObject _DoorLock;
    [SerializeField] private bool _isOpen;
    [SerializeField] private bool _isLock = true;
    private AgentActions _action;

    private new void Start()
    {
        base.Start();
        _interactiveObjectRecorder = GetComponent<InteractiveObjectRecorder>();
        _interactiveObjectRecorder.SetStatus(_isOpen);
        m_IsActivated = true;
        _DoorLock.GetComponent<Renderer>().material.color = _isLock ? Color.red : Color.green;

        if (_isOpen)
        {
            Open();
        }
        else
        {
            Close();
        }
    }

    void Open()
    {
        transform.rotation = Quaternion.Euler(0, 90, 0);
    }

    void Close()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        _action = other.GetComponent<AgentActions>();
        if (_action && _action.enabled)
        {
            m_HUD.ShowActionPrompt(_interactiveObjectRecorder.GetStatus() ? "Close door." : "Open door.");
            _action.SetInteract(true);
            _action.SetInteractionObject(this);
        }
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
        m_HUD.HideActionPrompt();
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
}
