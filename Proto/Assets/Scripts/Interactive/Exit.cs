using UnityEngine;

public class Exit : Interactive
{
    GameManager m_GameManager;
    GameObject m_Player;
    PhotonView m_PhotonView;

    protected new void Start ()
    {
        base.Start();
        m_PhotonView = GetComponent<PhotonView>();
        m_GameManager = FindObjectOfType<GameManager>();
	}

    void OnTriggerEnter(Collider other)
    {
        Action act = other.GetComponent<Action>();
        if (act)
        {
            if(act.enabled)
            {
                m_Player = other.gameObject;
                m_HUD.ShowActionPrompt("Exit mission");
                act.SetInteract(true);
                act.SetInteractionObject(this);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        Action act = other.GetComponent<Action>();
        if (act)
        {
            if(act.enabled)
            {
                m_HUD.HideActionPrompt();
                act.SetInteract(false);
            }        
        }
    }

    public override void Interact()
    {
        m_Player.SetActive(false);
        string msg = "";
        if (m_GameManager.ObjectivesCompleted())
        {
            msg = "Mission Successfull";
            if(m_GameManager.GetInnocentTargetIntercepted() > 0)
            {
                msg += "\nYou killed " + m_GameManager.GetInnocentTargetIntercepted() + " innocents.";
            }            
        }
        else
        {
            msg = "Mission Failed";
            msg += "\nYou didn't complete your objectives.";
        }

        m_PhotonView.RPC("RPCInteract", PhotonTargets.All, msg, 5f);
    }

    [PunRPC]
    void RPCInteract(string msg, float duration)
    {
        m_HUD.ShowMessages(msg,duration);

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
}