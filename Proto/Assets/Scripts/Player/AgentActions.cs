using UnityEngine;

public class AgentActions : Action
{
    MainRecorder m_Recorder;
    PhotonView m_PhotonView;
    GameManager m_GameManager;
    HUD m_Hud;

    new void Start()
    {
        base.Start();
        m_Recorder = FindObjectOfType<MainRecorder>();
        m_PhotonView = GetComponent<PhotonView>();
        m_GameManager = FindObjectOfType<GameManager>();
        m_Hud = FindObjectOfType<HUD>();
    }

    void Update()
    {
        if (m_Interact && Input.GetButtonDown("Action") && m_Recorder.IsRecording)
        {
            m_Interactive.Interact();
            m_Interact = false;
        }
    }
}