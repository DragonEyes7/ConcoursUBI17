using UnityEngine;

public class ClueGiver : Interactive
{
    [SerializeField]int _TargetID = 0;
    [SerializeField]string[] _PartsName;
    [SerializeField]Transform _USBPort;
    [SerializeField]string _USBObjectName;
    Material[] _Mats;
    GameManager m_GameManager;
    PhotonView m_PhotonView;
    Action m_Action;
    InteractiveObjectRecorder _interactiveObjectRecorder;

    new void Start()
    {
        base.Start();
        m_GameManager = FindObjectOfType<GameManager>();
        m_PhotonView = GetComponent<PhotonView>();
        _Mats = new Material[2];
        _Mats[0] = Resources.Load<Material>("MAT_OutlineAgent");
        _Mats[1] = Resources.Load<Material>("MAT_OutlineIntelligence");
        _interactiveObjectRecorder = GetComponent<InteractiveObjectRecorder>();
        _interactiveObjectRecorder.SetStatus(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (_interactiveObjectRecorder.GetStatus())
        {
            m_Action = other.GetComponent<IntelligenceAction>();

            if (m_Action && m_Action.enabled)
            {
                m_SelectMat = _Mats[1];
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
                m_SelectMat = _Mats[0];
                m_HUD.ShowActionPrompt("Install device");
                m_Action.SetInteract(true);
                m_Action.SetInteractionObject(this);
                Select();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (_interactiveObjectRecorder.GetStatus())
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
        if(PhotonNetwork.isMasterClient)
            _interactiveObjectRecorder.ObjectInteraction(!_interactiveObjectRecorder.GetStatus());
        else
            MoveObject();
    }

    public override void MoveObject()
    {
        if(_interactiveObjectRecorder.GetStatus() && m_Action != null && m_Action.isInteracting && !PhotonNetwork.isMasterClient)
        {
            foreach (var part in _PartsName)
            {
                m_PhotonView.RPC("SendClueToIntelligence", PhotonTargets.All, _TargetID, part);
            }
            m_HUD.BlinkUplink();
        }
        else
        {
            if (PhotonNetwork.isMasterClient)
            {
                var USB = PhotonNetwork.Instantiate(_USBObjectName, _USBPort.position, _USBPort.rotation, 0);
                USB.transform.SetParent(_USBPort);
            }
        }
        
        m_HUD.HideActionPrompt();
        m_Action.SetInteract(false);
        UnSelect();
    }

    public override void ResetObject()
    {
        foreach (Transform usb in _USBPort)
        {
            PhotonNetwork.Destroy(usb.gameObject);
        }
        if(m_Action)
            m_Action.SetInteract(true);
    }

    [PunRPC]
    void SendClueToIntelligence(int targetID, string part)
    {
        m_GameManager.AddCluesToIntelligence(part, m_GameManager.GetTargetClue(part));
    }
}
