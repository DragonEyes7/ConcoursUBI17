using UnityEngine;

public class MovableObject : Interactive
{
    [SerializeField]Transform[] m_PathToFollow;

    int m_CurrentPosition = 0;

    bool m_Move;

    new void Start()
    {
        base.Start();
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