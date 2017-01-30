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
        Debug.Log(m_Interactive);
        m_Recorder = GetComponent<Recorder>();
        m_Interact = false;
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