using UnityEngine;

public class IntelligenceAction : Action
{
    [SerializeField]Transform m_CenterCam;

    void Start()
    {

    }

    new void Update()
    {
        if (m_Interact && Input.GetButtonDown("Action"))
        {
            m_Interactive.Interact();
            m_Interact = false;
        }
    }

    public Transform GetCenterCam()
    {
        return m_CenterCam;
    }
}