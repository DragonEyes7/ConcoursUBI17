using UnityEngine;

abstract public class Interactive : MonoBehaviour
{
    protected Renderer[] m_Renderers;
    protected Material m_SelectMat;
    protected Material[] m_TargetMaterials;
    protected Material[][] m_TargetDefaultMaterial;

    protected HUD m_HUD;

    protected bool m_IsActivated = false;
    protected bool m_IsSelected = false;

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

    abstract public void Interact();

    public bool isActivated
    {
        get { return m_IsActivated; }
    }

    protected void Select()
    {
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
        m_IsSelected = false;
        if (m_Renderers.Length > 0)
        {
            for (int i = 0; i < m_Renderers.Length; ++i)
            {
                m_Renderers[i].materials = m_TargetDefaultMaterial[i];
            }
        }
    }
}