using UnityEngine;

public class Action : MonoBehaviour
{
    Interactive m_Interactive;
    bool m_Interact;

    public bool isInteracting
    {
        get { return m_Interact; }
    }

	void Start ()
    {
		
	}
	
	void Update ()
    {
		if(m_Interact && Input.GetButtonDown("Action"))
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