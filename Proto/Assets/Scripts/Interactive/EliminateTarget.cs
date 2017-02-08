﻿using UnityEngine;

public class EliminateTarget : Interactive
{
    GameManager m_GameManager;
    PhotonView m_PhotonView;
    AgentActions m_Action;

    new void Start()
    {
        base.Start();
        m_GameManager = FindObjectOfType<GameManager>();
        m_SelectMat = Resources.Load<Material>("MAT_OutlineAgent");
        m_PhotonView = GetComponent<PhotonView>();
    }

    void OnTriggerEnter(Collider other)
    {
        m_Action = other.GetComponent<AgentActions>();
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
        m_Action = other.GetComponent<AgentActions>();
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
        m_GameManager.ValidateTarget(GetComponent<Characteristics>().GetCharacteristics());
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
            Debug.Log("Wrong Target!");
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
}