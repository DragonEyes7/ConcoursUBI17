using UnityEngine;

public class Door : Interactive
{
    private DoorRecorder _doorRecorder;
    [SerializeField] private bool _isOpen;
    [SerializeField] private bool _isLock = true;
    private AgentActions _action;

    private new void Start()
    {
        base.Start();
        _doorRecorder = GetComponent<DoorRecorder>();
        _doorRecorder.SetDoorStatus(_isOpen);
        m_IsActivated = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        _action = other.GetComponent<AgentActions>();
        if (_action && _action.enabled)
        {
            m_HUD.ShowActionPrompt(_isOpen ? "Close door." : "Open door.");
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

    public override void Interact()
    {
        if(!_isLock)
        {
            _doorRecorder.DoorInteraction(!_doorRecorder.GetDoorStatus());
        }
        else
        {
            FindObjectOfType<HUD>().ShowMessages("Door is locked", 3f);
        }
    }

    public void Unlock()
    {
        GetComponent<PhotonView>().RPC("RPCUnlock", PhotonTargets.All);
    }

    [PunRPC]
    void RPCUnlock()
    {
        _isLock = false;
    }
}
