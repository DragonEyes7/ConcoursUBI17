using UnityEngine;

public class ClueGiver : Interactive
{
    [SerializeField]int _TargetID = 0;
    [SerializeField]string[] _PartsName;
    GameManager m_GameManager;
    PhotonView m_PhotonView;
    AgentActions m_Action;
    private InteractiveObjectRecorder _interactiveObjectRecorder;

    new void Start()
    {
        base.Start();
        m_GameManager = FindObjectOfType<GameManager>();
        m_PhotonView = GetComponent<PhotonView>();
        _interactiveObjectRecorder = GetComponent<InteractiveObjectRecorder>();
    }

    void OnTriggerEnter(Collider other)
    {
        m_Action = other.GetComponent<AgentActions>();
        if (m_Action && !m_IsActivated)
        {
            if (m_Action.enabled)
            {
                m_HUD.ShowActionPrompt("Search for clues");
                m_Action.SetInteract(true);
                m_Action.SetInteractionObject(this);
                Select();
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
        m_IsActivated = true;

        for(int i = 0; i < _PartsName.Length; ++i)
        {
            m_PhotonView.RPC("SendClueToAgent", PhotonTargets.All, _TargetID, _PartsName[i]);
        }

        m_HUD.HideActionPrompt();
        m_Action.SetInteract(false);
        UnSelect();

        m_HUD.ShowUplink(true);
    }

    public override void ResetObject()
    {
        m_IsActivated = false;
        if(m_Action)
            m_Action.SetInteract(true);
        m_GameManager.ClearAgentClues();
    }

    [PunRPC]
    void SendClueToAgent(int targetID, string part)
    {
        m_GameManager.AddToAgentClues(part, m_GameManager.GetTargetClue(targetID, part));
    }
}
