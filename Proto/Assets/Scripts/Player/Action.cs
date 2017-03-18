using UnityEngine;

public class Action : MonoBehaviour
{
    protected Interactive m_Interactive;
    protected bool m_Interact;
    protected bool _intercept;

    public bool isInteracting
    {
        get { return m_Interact; }
    }

    public bool isIntercepting
    {
        get { return _intercept; }
    }

	protected void Start ()
    {
        m_Interact = false;
        _intercept = false;
    }

    public void SetInteract(bool value)
    {
        m_Interact = value;
    }

    public void SetIntercept(bool value)
    {
        _intercept = value;
    }

    public void SetInteractionObject(Interactive target)
    {
        m_Interactive = target;
    }
}