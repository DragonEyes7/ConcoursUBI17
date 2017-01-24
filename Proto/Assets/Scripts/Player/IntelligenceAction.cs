using UnityEngine;

public class IntelligenceAction : MonoBehaviour
{
    Renderer[] m_Renderers;
    Material m_SelectMat;
    Material[] m_TargetMaterials;
    Material[][] m_TargetDefaultMaterial;

    Interactive m_FocusInteractive;

    bool m_IsActive = false;

    void Start()
    {
        m_SelectMat = Resources.Load<Material>("MAT_OutlineOrange");
    }

    void Update()
    {
        if(Input.GetButtonDown("Action") && m_FocusInteractive)
        {
            m_FocusInteractive.Interact();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(m_IsActive)
        {
            Terminal term = other.GetComponent<Terminal>();
            if(term)
            {
                RaycastHit hit;

                if (Physics.Raycast(transform.position, transform.forward, out hit, 10f))
                {
                    if (hit.transform == term.transform)
                    {
                        if (!term.isActivated)
                        {
                            SetupHighlight(term);
                            Highlight();
                        }                        
                    }
                }
            }
        }        
    }

    void OnTriggerExit(Collider other)
    {
        if (m_IsActive && m_FocusInteractive)
        {
            if (m_FocusInteractive.gameObject == other.gameObject)
            {
                UnHighlight();
            }
        }
    }

    void SetupHighlight(Interactive objectToHightlight)
    {
        m_FocusInteractive = objectToHightlight;
        Debug.Log(m_FocusInteractive);
        m_Renderers = objectToHightlight.gameObject.GetComponents<Renderer>();

        m_TargetMaterials = new Material[2];
        m_TargetDefaultMaterial = new Material[2][];

        if (m_Renderers.Length > m_TargetDefaultMaterial.Length)
        {
            m_TargetDefaultMaterial = new Material[m_Renderers.Length][];
        }
    }

    void Highlight()
    {
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

    void UnHighlight()
    {
        if (m_Renderers.Length > 0)
        {
            for (int i = 0; i < m_Renderers.Length; ++i)
            {
                m_Renderers[i].materials = m_TargetDefaultMaterial[i];
            }
        }

        m_FocusInteractive = null;
    }

    public void SetActive(bool value)
    {
        m_IsActive = value;
    }
}