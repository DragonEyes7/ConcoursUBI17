using UnityEngine;

public class MovableObject : Interactive
{
    [SerializeField]Transform[] m_PathToFollow;
    Renderer[] m_Renderers;
    Material m_SelectMat;
    Material[] m_TargetMaterials;
    Material[][] m_TargetDefaultMaterial;

    int m_CurrentPosition = 0;

    bool m_Move;

    new void Start()
    {
        base.Start();
        m_SelectMat = Resources.Load<Material>("MAT_OutlineOrange");

        m_Renderers = GetComponentsInChildren<Renderer>();

        m_TargetMaterials = new Material[2];
        m_TargetDefaultMaterial = new Material[2][];

        if (m_Renderers.Length > m_TargetDefaultMaterial.Length)
        {
            m_TargetDefaultMaterial = new Material[m_Renderers.Length][];
        }
    }

    void Update()
    {
        if(m_Move && m_CurrentPosition < m_PathToFollow.Length)
        {
            Move();
        }
    }

    void Move()
    {
        if(Vector3.Distance(m_PathToFollow[m_CurrentPosition].position, transform.position) < 0.1f)
        {
            ++m_CurrentPosition;
            if(m_CurrentPosition >= m_PathToFollow.Length)
            {
                m_CurrentPosition = m_PathToFollow.Length;
                return;
            }
        }

        transform.position = Vector3.Lerp(transform.position, m_PathToFollow[m_CurrentPosition].position, 0.2f);
    }

    void OnTriggerEnter(Collider other)
    {
        if(!m_Move)
        {
            Action act = other.GetComponent<Action>();
            if (act)
            {
                if (act.enabled)
                {
                    m_HUD.ShowActionPrompt("Move Chair");
                    act.SetInteract(true);
                    act.SetInteractionObject(this);
                    Select();
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        Action act = other.GetComponent<Action>();
        if (act)
        {
            if (act.enabled)
            {
                m_HUD.HideActionPrompt();
                act.SetInteract(false);
                UnSelect();
            }
        }
    }

    void Select()
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

    void UnSelect()
    {
        if (m_Renderers.Length > 0)
        {
            for (int i = 0; i < m_Renderers.Length; ++i)
            {
                m_Renderers[i].materials = m_TargetDefaultMaterial[i];
            }
        }
    }

    public override void Interact()
    {
        GetComponent<PhotonView>().RPC("RPCInteract", PhotonTargets.All);
    }

    [PunRPC]
    void RPCInteract()
    {
        m_Move = true;
    }
}