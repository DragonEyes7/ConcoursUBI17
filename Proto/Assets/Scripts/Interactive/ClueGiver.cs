using System;
using UnityEngine;

public class ClueGiver : Interactive
{
    [SerializeField]Door[] m_Doors;

    void OnTriggerEnter(Collider other)
    {
        Action act = other.GetComponent<Action>();
        if (act)
        {
            if (act.enabled)
            {
                m_HUD.ShowActionPrompt("Search for clues");
                act.SetInteract(true);
                act.SetInteractionObject(this);
                Select();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        Action act = other.GetComponent<Action>();
        if (act)
        {
            if (act.enabled)
            {
                m_HUD.HideActionPrompt();
                act.SetInteract(false);
                UnSelect();
            }
        }
    }

    public override void Interact()
    {
        foreach (Door door in m_Doors)
        {
            door.Interact();
        }

        m_IsActivated = true;

        Debug.Log("The target is the only person exept you in the level!");
    }
}
