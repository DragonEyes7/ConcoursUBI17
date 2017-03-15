using UnityEngine;

public class ClueGiver : Interactive
{
    [SerializeField]string[] _PartsName;
    [SerializeField]Transform _USBPort;
    [SerializeField]string _USBObjectName;
    [SerializeField]AudioClip[] _AudioClips;
    Material[] _Mats;
    GameManager m_GameManager;
    PhotonView m_PhotonView;
    Action m_Action;
    InteractiveObjectRecorder _interactiveObjectRecorder;
    bool _hasClue = false, _hasUSB = false;

    AudioSource _AudioSource;

    new void Start()
    {
        base.Start();
        _AudioSource = GetComponent<AudioSource>();
        m_GameManager = FindObjectOfType<GameManager>();
        m_PhotonView = GetComponent<PhotonView>();
        _Mats = new Material[2];
        _Mats[0] = Resources.Load<Material>("MAT_OutlineAgent");
        _Mats[1] = Resources.Load<Material>("MAT_OutlineIntelligence");
        _interactiveObjectRecorder = GetComponent<InteractiveObjectRecorder>();
        _interactiveObjectRecorder.SetStatus(false);
    }

    void OnTriggerStay(Collider other)
    {
        if (_interactiveObjectRecorder.GetStatus() && !_hasClue && !PhotonNetwork.isMasterClient)
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
        else if(PhotonNetwork.isMasterClient && !hasUSB())
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
        Debug.Log("Clue:" + _hasClue + ": USB" + hasUSB());
        if(!_hasClue && hasUSB())
        {
            foreach (var part in _PartsName)
            {
                m_PhotonView.RPC("SendClueToIntelligence", PhotonTargets.All, part);
            }
            m_HUD.BlinkUplink();
            _hasClue = true;
        }
        else if (PhotonNetwork.isMasterClient)
        {
            var USB = PhotonNetwork.Instantiate(_USBObjectName, _USBPort.position, _USBPort.rotation, 0);
            USB.transform.SetParent(_USBPort);
            m_PhotonView.RPC("RPCSetHasUSB", PhotonTargets.All, true);
        }

        _AudioSource.clip = _AudioClips[0];
        _AudioSource.Play();

        m_HUD.HideActionPrompt();
        UnSelect();
    }

    [PunRPC]
    void RPCSetHasUSB(bool hasUSB)
    {
        _hasUSB = hasUSB;
    }

    bool hasUSB()
    {
        return _hasUSB;
    }

    public override void ResetObject()
    {
        foreach (Transform usb in _USBPort)
        {
            PhotonNetwork.Destroy(usb.gameObject);
        }
        m_PhotonView.RPC("RPCSetHasUSB", PhotonTargets.All, false);
    }

    public void SetPartsName(string[] partsName)
    {
        _PartsName = partsName;
    }

    [PunRPC]
    void SendClueToIntelligence(string part)
    {
        _AudioSource.clip = _AudioClips[1];
        _AudioSource.Play();
        m_GameManager.AddCluesToIntelligence(part, m_GameManager.GetTargetClue(part));
    }
}
