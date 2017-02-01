using UnityEngine;

public class AgentActions : Action
{
    Recorder m_Recorder;
    PhotonView m_PhotonView;

    new void Start()
    {
        base.Start();
        m_Recorder = GetComponent<Recorder>();
        m_PhotonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (m_Interact && Input.GetButtonDown("Action") && m_Recorder.isRecording)
        {
            m_Interactive.Interact();
            m_Interact = false;
        }

        if (Input.GetButtonDown("Uplink"))
        {
            Debug.Log("Send clues to intelligence");
            m_PhotonView.RPC("Uplink", PhotonTargets.All);
        }
    }

    [PunRPC]
    void Uplink()
    {
        FindObjectOfType<GameManager>().SendCluesToIntelligence();
    }
}
