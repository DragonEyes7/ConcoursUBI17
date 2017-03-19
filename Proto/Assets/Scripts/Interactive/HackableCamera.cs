using UnityEngine;

public class HackableCamera : Interactive
{
    [SerializeField]LayerMask m_Layer;
    [SerializeField]string m_CameraGroup = "";
    IntelligenceAction m_Action;

    CamerasController m_CamerasController;

    new void Start()
    {
        base.Start();

        m_SelectMat = Resources.Load<Material>("MAT_OutlineIntelligence");

        m_CamerasController = FindObjectOfType<CamerasController>();
    }

    public string CameraGroup()
    {
        return m_CameraGroup;
    }

    void OnTriggerStay(Collider other)
    {
        if (!m_IsActivated && !m_IsSelected)
        {
            m_Action = other.GetComponent<IntelligenceAction>();
            if (m_Action)
            {
                if (m_Action.enabled)
                {
                    RaycastHit hit;

                    Vector3 direction = m_Action.GetCenterCam().position - transform.position;

                    if (Physics.Raycast(transform.position, direction, out hit, 50f, m_Layer))
                    {
                        Debug.DrawRay(transform.position, direction, Color.red, 5f);
                        if (hit.transform == m_Action.GetCenterCam().transform)
                        {
                            m_HUD.ShowActionPrompt("Hack Camera");
                            m_Action.SetInteract(true);
                            m_Action.SetInteractionObject(this);
                            Select();
                        }
                    }
                }
            }
        }
        else if(m_IsSelected)
        {

            RaycastHit hit;

            Vector3 direction = m_Action.GetCenterCam().position - transform.position;

            if (Physics.Raycast(transform.position, direction, out hit, 50f, m_Layer))
            {
                Debug.DrawRay(transform.position, direction, Color.red, 5f);
                if (hit.transform != m_Action.GetCenterCam().transform)
                {
                    UnSelect();
                    m_HUD.HideActionPrompt();
                    if (!m_Action) return;
                    m_Action.SetInteract(false);
                    m_Action.SetInteractionObject(null);
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
                }
            }
        }
    }

    public override void Interact()
    {
        m_CamerasController.TakeControl(gameObject, m_CameraGroup);

        UnSelect();

        //m_IsActivated = true;

        if (m_Action)
        {
            m_HUD.HideActionPrompt();
            m_Action.SetInteract(false);
            m_Action = null;
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
