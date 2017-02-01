using UnityEngine;

public class Action : MonoBehaviour
{
    protected Interactive m_Interactive;
    protected bool m_Interact;

    public bool isInteracting
    {
        get { return m_Interact; }
    }

	protected void Start ()
    {
        m_Interact = false;
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