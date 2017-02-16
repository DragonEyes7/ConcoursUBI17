using UnityEngine;

public class ClueGiver : Interactive
{
    [SerializeField]int _TargetID = 0;
    [SerializeField]string[] _PartsName;
    [SerializeField]Transform _USBPort;
    [SerializeField]string _USBObjectName;
    GameManager m_GameManager;
    PhotonView m_PhotonView;
    Action m_Action;
    InteractiveObjectRecorder _interactiveObjectRecorder;

    new void Start()
    {
        base.Start();
        m_GameManager = FindObjectOfType<GameManager>();
        m_PhotonView = GetComponent<PhotonView>();
        _interactiveObjectRecorder = GetComponent<InteractiveObjectRecorder>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (m_IsActivated)
        {
            m_Action = other.GetComponent<IntelligenceAction>();

            if (m_Action && m_Action.enabled)
            {
                m_HUD.ShowActionPrompt("Hack device");
                m_Action.SetInteract(true);
                m_Action.SetInteractionObject(this);
                Select();
            }
        }
        else
        {
            m_Action = other.GetComponent<AgentActions>();

            if (m_Action && m_Action.enabled)
            {
                m_HUD.ShowActionPrompt("Install device");
                m_Action.SetInteract(true);
                m_Action.SetInteractionObject(this);
                Select();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (m_IsActivated)
        {
            m_Action = other.GetComponent<IntelligenceAction>();

            if (m_Action && m_Action.enabled)
            {
                m_HUD.HideActionPrompt();
                m_Action.SetInteract(false);
                UnSelect();
            }
        }
        else
        {
            m_Action = other.GetComponent<AgentActions>();

            if (m_Action && m_Action.enabled)
            {
                m_HUD.HideActionPrompt();
                m_Action.SetInteract(false);
                UnSelect();
            }
        }
    }

    public override void Interact()
    {
        _interactiveObjectRecorder.ObjectInteraction(!_interactiveObjectRecorder.GetStatus());
    }

    public override void MoveObject()
    {
        if(m_IsActivated)
        {
            for (int i = 0; i < _PartsName.Length; ++i)
            {
                m_PhotonView.RPC("SendClueToIntelligence", PhotonTargets.All, _TargetID, _PartsName[i]);
            }
            m_HUD.BlinkUplink();
        }
        else
        {
            if (PhotonNetwork.isMasterClient)
            {
                GameObject USB = PhotonNetwork.Instantiate(_USBObjectName, _USBPort.position, _USBPort.rotation, 0);
                USB.transform.SetParent(_USBPort);
                m_PhotonView.RPC("SetUSB", PhotonTargets.All, true);
            }
        }
        
        m_HUD.HideActionPrompt();
        m_Action.SetInteract(false);
        UnSelect();
    }

    public override void ResetObject()
    {
        /*
         * Currently bug for the intelligence action need to not reset the object when the
         * intelligence interact with this!
        if(PhotonNetwork.isMasterClient)
        {
            PhotonNetwork.Destroy(_USBPort.GetChild(0).gameObject);
        } */       
    }

    [PunRPC]
    void SetUSB(bool value)
    {
        m_IsActivated = value;
    }

    [PunRPC]
    void SendClueToIntelligence(int targetID, string part)
    {
        m_GameManager.AddCluesToIntelligence(part, m_GameManager.GetTargetClue(targetID, part));
    }
}
