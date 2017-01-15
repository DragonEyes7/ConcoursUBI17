using UnityEngine;

public class Exit : Interactive
{
    GameManager m_GameManager;
    GameObject m_Player;

    protected new void Start ()
    {
        base.Start();

        m_GameManager = FindObjectOfType<GameManager>();
	}

    void OnTriggerEnter(Collider other)
    {
        Action act = other.GetComponent<Action>();
        if (act)
        {
            m_Player = other.gameObject;
            m_HUD.ShowActionPrompt("Exit mission");
            act.SetInteract(true);
            act.SetInteractionObject(this);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Action act = other.GetComponent<Action>();
        if (act)
        {
            m_HUD.HideActionPrompt();
            act.SetInteract(false);            
        }
    }

    public override void Interact()
    {
        m_Player.SetActive(false);
        if (m_GameManager.ObjectivesCompleted())
        {
            string msg = "Mission Successfull";
            if(m_GameManager.GetInnocentTargetKilled() > 0)
            {
                msg += "\nYou killed " + m_GameManager.GetInnocentTargetKilled() + " innocents.";
            }

            m_HUD.ShowMessages(msg, 5f);
        }
        else
        {
            string msg = "Mission Failed";
            msg += "\nYou didn't complete your objectives.";
            m_HUD.ShowMessages(msg, 5f);
        }
    }
}