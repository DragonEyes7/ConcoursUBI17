using UnityEngine;

public class Bomb : Interactive
{
    GameManager m_GameManager;
    PhotonView m_PhotonView;
    Action m_Action;

    new void Start()
    {
        base.Start();
        m_GameManager = FindObjectOfType<GameManager>();
        m_SelectMat = Resources.Load<Material>("MAT_OutlineAgent");
        m_PhotonView = GetComponent<PhotonView>();
    }

	void Update ()
    {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (!m_IsActivated)
        {
            m_Action = other.GetComponent<Action>();
            if (m_Action)
            {
                if (m_Action.enabled)
                {
                    m_HUD.ShowActionPrompt( m_GameManager.ObjectivesCompleted() ? "Defuse Bomb" : "Inspect Bomb");
                    m_Action.SetInteract(true);
                    m_Action.SetInteractionObject(this);
                    Select();
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        m_Action = other.GetComponent<Action>();
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

    [PunRPC]
    void RPCInteract(string msg, float duration)
    {
        m_HUD.ShowMessages(msg, duration);

        if (m_GameManager.isMaster)
        {
            duration += 1f;
        }

        Invoke("Disconnect", duration + 1f);
    }

    void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    public override void Interact()
    {
        if (m_GameManager.ObjectivesCompleted())
        {
            string msg = "Mission Successfull";
            m_PhotonView.RPC("RPCInteract", PhotonTargets.All, msg, 5f);
            m_HUD.HideActionPrompt();
            m_Action.SetInteract(false);
            UnSelect();
        }
        else
        {

        }
    }

    public override void MoveObject()
    {
        //TODO This here should never be called, as of now at least, probably better to leave it as is, if we ever need it
        throw new System.NotImplementedException();
    }

    public override void ResetObject()
    {
        //TODO This here should never be called, as of now at least, probably better to leave it as is, if we ever need it
        throw new System.NotImplementedException();
    }
}