using UnityEngine;

public class Terminal : Interactive
{
    [SerializeField]LayerMask m_Layer;
    [SerializeField]Door[] m_Doors;

    IntelligenceAction m_Action;

    new void Start()
    {
        base.Start();

        m_SelectMat = Resources.Load<Material>("MAT_OutlineIntelligence");
    }

    void OnTriggerEnter(Collider other)
    {
        if (!m_IsActivated)
        {
            m_Action = other.GetComponent<IntelligenceAction>();
            if (m_Action && m_Action.enabled)
            {
                RaycastHit hit;
                    
                Vector3 direction = m_Action.GetCenterCam().position - transform.position;

                if (Physics.Raycast(transform.position, direction, out hit, 25f, m_Layer))
                {
                    Debug.DrawRay(transform.position, direction, Color.red, 5f);
                    if(hit.transform == m_Action.GetCenterCam().transform)
                    {
                        m_HUD.ShowActionPrompt("Hack Terminal");
                        m_Action.SetInteract(true);
                        m_Action.SetInteractionObject(this);
                        Select();
                    }
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!m_IsActivated && m_IsSelected)
        {
            m_Action = other.GetComponent<IntelligenceAction>();
            if (m_Action)
            {
                if (m_Action.enabled)
                {
                    UnSelect();
                    m_HUD.HideActionPrompt();
                    m_Action.SetInteract(false);
                    m_Action.SetInteractionObject(null);
                    m_Action = null;
                }
            }
        }
    }

    public override void Interact()
    {
        foreach (Door door in m_Doors)
        {
            door.Unlock();
        }

        m_IsActivated = true;

        if (m_Action)
        {
            UnSelect();
            m_HUD.HideActionPrompt();
            m_Action.SetInteract(false);
        }
    }
}