using UnityEngine;

public class Door : Interactive
{
    [SerializeField]bool m_IsOpen = false;
    [SerializeField]bool m_IsLock = true;
    AgentActions m_Action;

    new void Start()
    {
        base.Start();

        m_IsActivated = true;
    }

    void OnTriggerEnter(Collider other)
    {
        m_Action = other.GetComponent<AgentActions>();
        if (m_Action)
        {
            if (m_Action.enabled)
            {
                m_HUD.ShowActionPrompt("Open door.");
                m_Action.SetInteract(true);
                m_Action.SetInteractionObject(this);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        m_Action = other.GetComponent<AgentActions>();
        if (m_Action)
        {
            if (m_Action.enabled)
            {
                m_HUD.HideActionPrompt();
                m_Action.SetInteract(false);
            }
        }
    }

    public override void Interact()
    {
        if(!m_IsLock)
        {
            if(m_IsOpen)
            {
                m_IsOpen = false;
                CloseDoor();
            }
            else
            {
                m_IsOpen = true;
                OpenDoor();
            }
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

    void OpenDoor()
    {
        GetComponent<PhotonView>().RPC("RPCOpenDoor", PhotonTargets.All);
    }

    void CloseDoor()
    {
        GetComponent<PhotonView>().RPC("RPCCloseDoor", PhotonTargets.All);
    }

    [PunRPC]
    void RPCOpenDoor()
    {
        transform.rotation = Quaternion.Euler(0,90,0);
    }

    [PunRPC]
    void RPCCloseDoor()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    [PunRPC]
    void RPCUnlock()
    {
        m_IsLock = false;
    }
}
