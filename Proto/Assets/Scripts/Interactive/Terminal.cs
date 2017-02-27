using System;
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
                        Select();
                    }
                }
            }
        }
    }

    protected override void Select()
    {
        m_HUD.ShowActionPrompt("Hack Terminal");
        if (m_Action)
        {
            m_Action.SetInteract(true);
            m_Action.SetInteractionObject(this);
        }
        base.Select();
    }

    protected override void UnSelect()
    {
        base.UnSelect();
        m_HUD.HideActionPrompt();
        if (m_Action)
        {
            m_Action.SetInteract(false);
            m_Action.SetInteractionObject(null);
            m_Action = null;
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
        }
    }

    public override void MoveObject()
    {
        //TODO This here should never be called, as of now at least, probably better to leave it as is, if we ever need it
        throw new System.NotImplementedException();
    }

    public override void ResetObject()
    {
        //TODO This here should never be called, as of now at least, probably better to leave it as is, if we ever need it
        throw new System.NotImplementedException();
    }
}