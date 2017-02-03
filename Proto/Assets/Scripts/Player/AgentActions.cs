using UnityEngine;

public class AgentActions : Action
{
    Recorder m_Recorder;
    PhotonView m_PhotonView;
    GameManager m_GameManager;
    HUD m_Hud;

    new void Start()
    {
        base.Start();
        m_Recorder = GetComponent<Recorder>();
        m_PhotonView = GetComponent<PhotonView>();
        m_GameManager = FindObjectOfType<GameManager>();
        m_Hud = FindObjectOfType<HUD>();
    }

    void Update()
    {
        if (m_Interact && Input.GetButtonDown("Action") && m_Recorder.isRecording)
        {
            m_Interactive.Interact();
            m_Interact = false;
        }

        if (m_GameManager.AgentHasClues() && Input.GetButtonDown("Uplink"))
        {
            m_PhotonView.RPC("Uplink", PhotonTargets.All);
            m_Hud.ShowUplink(false);
        }
    }

    [PunRPC]
    void Uplink()
    {
        FindObjectOfType<GameManager>().SendCluesToIntelligence();

        if(PhotonNetwork.isNonMasterClientInRoom)
        {
            FindObjectOfType<HUD>().BlinkUplink();
        }
    }
}