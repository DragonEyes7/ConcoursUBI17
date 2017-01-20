using UnityEngine;

public class EliminateTarget : Interactive
{	
    void OnTriggerEnter(Collider other)
    {
        Action act = other.GetComponent<Action>();
        if(act)
        {
            if(act.enabled)
            {
                m_HUD.ShowActionPrompt("Eliminate");
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
        PhotonNetwork.Destroy(gameObject);
    }
}