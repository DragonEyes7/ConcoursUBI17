using UnityEngine;

public abstract class Interactive : MonoBehaviour
{
    protected Renderer[] m_Renderers;
    protected Material m_SelectMat;
    protected Material[] m_TargetMaterials;
    protected Material[][] m_TargetDefaultMaterial;

    protected HUD m_HUD;

    protected bool m_IsActivated = false;
    protected bool m_IsSelected;

    protected void Start()
    {
        m_HUD = FindObjectOfType<HUD>();

        m_Renderers = GetComponentsInChildren<Renderer>();

        m_TargetMaterials = new Material[2];
        m_TargetDefaultMaterial = new Material[2][];

        if (m_Renderers.Length > m_TargetDefaultMaterial.Length)
        {
            m_TargetDefaultMaterial = new Material[m_Renderers.Length][];
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!m_IsActivated)
        {
            var action = other.GetComponent<IntelligenceAction>();
            if (action && action.enabled)
            {
                RaycastHit hit;

                var direction = action.GetCenterCam().position - transform.position;

                if (Physics.Raycast(transform.position, direction, out hit, 25f, 1))
                {
                    Debug.DrawRay(transform.position, direction, Color.red, 5f);
                    if (hit.transform != action.GetCenterCam().transform)
                    {
                        UnSelect(action);
                    }
                    else
                    {
                        Select(action);
                    }
                }
            }
        }
    }

    public abstract void Interact();
    public virtual void Intercept(){}

    public abstract void MoveObject();
    public abstract void ResetObject();

    public bool isActivated
    {
        get { return m_IsActivated; }
    }

    protected virtual void Select(Action action)
    {
        Select();
    }

    protected virtual void UnSelect(Action action)
    {
        UnSelect();
    }

    protected void Select()
    {
        if (m_IsSelected) return;
        m_IsSelected = true;
        if (m_Renderers.Length > 0)
        {
            for (int i = 0; i < m_Renderers.Length; ++i)
            {
                m_TargetDefaultMaterial[i] = m_Renderers[i].materials;
                m_TargetMaterials[0] = m_TargetDefaultMaterial[i][0];
                m_TargetMaterials[1] = m_SelectMat;
                m_Renderers[i].materials = m_TargetMaterials;
            }
        }
    }

    protected void UnSelect()
    {
        if (!m_IsSelected) return;
        m_IsSelected = false;
        if (m_Renderers.Length > 0 && m_TargetDefaultMaterial.Length > 0)
        {
            for (int i = 0; i < m_Renderers.Length; ++i)
            {
                if (m_TargetDefaultMaterial[i] != null)
                {
                    m_Renderers[i].materials = m_TargetDefaultMaterial[i];
                }
            }
        }
    }
}