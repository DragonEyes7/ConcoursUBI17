using UnityEngine;

abstract public class Interactive : MonoBehaviour
{
    protected HUD m_HUD;

    protected bool m_IsActivated = false;

    protected void Start()
    {
        m_HUD = FindObjectOfType<HUD>();
    }

    abstract public void Interact();

    public bool isActivated
    {
        get { return m_IsActivated; }
    }
}