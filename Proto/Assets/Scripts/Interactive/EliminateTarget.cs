using UnityEngine;

public class EliminateTarget : Interactive
{
    Action m_Action;

    void OnTriggerEnter(Collider other)
    {
        m_Action = other.GetComponent<Action>();
        if(m_Action)
        {
            if(m_Action.enabled)
            {
                m_HUD.ShowActionPrompt("Intercept");
                m_Action.SetInteract(true);
                m_Action.SetInteractionObject(this);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        m_Action = other.GetComponent<Action>();
        if (m_Action)
        {
            if(m_Action.enabled)
            {
                m_HUD.HideActionPrompt();
                m_Action.SetInteract(false);
            }
        }
    }

    public override void Interact()
    {
        if(m_Action)
        {
            m_HUD.HideActionPrompt();
            m_Action.SetInteract(false);
        }

        PhotonNetwork.Destroy(gameObject);
    }
}