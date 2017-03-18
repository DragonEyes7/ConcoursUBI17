using UnityEngine;

public class AgentActions : Action
{
    MainRecorder m_Recorder;

    new void Start()
    {
        base.Start();
        m_Recorder = FindObjectOfType<MainRecorder>();
    }

    void Update()
    {
        if (m_Interact && Input.GetButtonDown("Action") && m_Recorder.IsRecording)
        {
            m_Interactive.Interact();
            m_Interact = false;
        }
        if (_intercept && Input.GetButtonDown("Uplink") && m_Recorder.IsRecording)
        {
            m_Interactive.Intercept();
            _intercept = false;
        }
    }
}