﻿using UnityEngine;

public class EliminateTarget : Interactive
{
    [SerializeField]AudioClip[] _PushClips;
    [SerializeField]AudioClip _WinningClip;
    GameManager m_GameManager;
    PhotonView m_PhotonView;
    AgentActions m_Action;
    AudioSource _AudioSource;

    new void Start()
    {
        base.Start();
        _AudioSource = GetComponent<AudioSource>();
        m_GameManager = FindObjectOfType<GameManager>();
        m_SelectMat = Resources.Load<Material>("MAT_OutlineAgent");
        m_PhotonView = GetComponent<PhotonView>();
    }

    void OnTriggerStay(Collider other)
    {
        m_Action = other.GetComponent<AgentActions>();
        if(m_Action)
        {
            if(m_Action.enabled)
            {
                if(Vector3.Distance(other.transform.position, transform.position) <= 0.5f)
                {
                    _AudioSource.clip = _PushClips[Random.Range(0, _PushClips.Length)];
                    _AudioSource.Play();
                }
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
        m_GameManager.ValidateTarget(GetComponent<NPCWalkScript>().NPCID);
        string msg;
        if (m_GameManager.ObjectivesCompleted())
        {
            _AudioSource.clip = _WinningClip;
            _AudioSource.spatialBlend = 0;
            _AudioSource.Play();
            msg = "Mission Successfull";
            m_PhotonView.RPC("RPCInteract", PhotonTargets.All, msg, 5f);
            m_HUD.HideActionPrompt();
            UnSelect();
        }
        else
        {
            msg = "Wrong Target, penality +1";
            m_PhotonView.RPC("RPCWrongTargetMessage", PhotonTargets.All, msg, 5f);
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

    [PunRPC]
    void RPCWrongTargetMessage(string msg, float duration)
    {
        m_HUD.ShowMessages(msg, duration);
        m_HUD.WrongTargetIntercepted();
    }

    [PunRPC]
    void RPCInteract(string msg, float duration)
    {
        m_HUD.ShowMessages(msg, duration);

        if (m_GameManager.isMaster)
        {
            duration += 1f;
        }

        m_HUD.GameEndedSuccessfully();

        Invoke("Disconnect", duration + 1f);
    }

    void Disconnect()
    {
        m_GameManager.Disconnect();
    }
}