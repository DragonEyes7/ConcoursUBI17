using UnityEngine;

public class Exit : Interactive
{
    GameManager m_GameManager;

    void Start ()
    {
        m_GameManager = FindObjectOfType<GameManager>();
	}

    void OnTriggerEnter(Collider other)
    {
        Action act = other.GetComponent<Action>();
        if (act)
        {
            Debug.Log("Player is leaving...");
            act.SetInteract(true);
            act.SetInteractionObject(this);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Action act = other.GetComponent<Action>();
        if (act)
        {
            Debug.Log("EliminateTarget not Possible");
            act.SetInteract(false);            
        }
    }

    public override void Interact()
    {
        if (m_GameManager.ObjectivesCompleted())
        {
            Debug.Log("Mission Successfull");
            if(m_GameManager.GetInnocentTargetKilled() > 0)
            {
                Debug.Log("You killed " + m_GameManager.GetInnocentTargetKilled() + " innocents.");
            }
        }
        else
        {
            Debug.Log("Mission failed");
            Debug.Log("You didn't complete your objectives.");
        }
    }
}