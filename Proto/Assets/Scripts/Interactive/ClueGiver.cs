using System;
using UnityEngine;

public class ClueGiver : Interactive
{
    [SerializeField]Door[] m_Doors;
    GameManager m_GameManager;
    PhotonView m_PhotonView;

    new void Start()
    {
        base.Start();
        m_GameManager = FindObjectOfType<GameManager>();
        m_PhotonView = GetComponent<PhotonView>();
    }

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

        m_PhotonView.RPC("SendClueToAgent", PhotonTargets.All, 0, "Nose");
        m_PhotonView.RPC("SendClueToAgent", PhotonTargets.All, 0, "Hair");
        m_PhotonView.RPC("SendClueToAgent", PhotonTargets.All, 0, "Backpack");

        m_HUD.ShowUplink(true);
    }

    [PunRPC]
    void SendClueToAgent(int targetID, string part)
    {
        m_GameManager.AddToAgentClues(part, m_GameManager.GetTargetClue(targetID, part));
    }
}
