using UnityEngine;

public class Action : MonoBehaviour
{
    Recorder m_Recorder;
    protected Interactive m_Interactive;
    protected bool m_Interact;

    public bool isInteracting
    {
        get { return m_Interact; }
    }

	void Start ()
    {
        m_Recorder = GetComponent<Recorder>();
	}
	
	protected void Update ()
    {
		if(m_Interact && Input.GetButtonDown("Action") && m_Recorder.isRecording)
        {
            m_Interactive.Interact();
            m_Interact = false;
        }
	}

    public void SetInteract(bool value)
    {
        m_Interact = value;
    }

    public void SetInteractionObject(Interactive target)
    {
        m_Interactive = target;
    }
}