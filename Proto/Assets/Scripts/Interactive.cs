using UnityEngine;

abstract public class Interactive : MonoBehaviour
{
    protected HUD m_HUD;

    protected void Start()
    {
        m_HUD = FindObjectOfType<HUD>();
    }

    abstract public void Interact();
}
