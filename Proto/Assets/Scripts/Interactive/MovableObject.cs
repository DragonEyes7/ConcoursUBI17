using UnityEngine;

public class MovableObject : Interactive
{
    [SerializeField]Transform[] m_PathToFollow;

    private InteractiveObjectRecorder _interactiveObjectRecorder;


    int m_CurrentPosition = 0;

    new void Start()
    {
        base.Start();
        m_SelectMat = Resources.Load<Material>("MAT_OutlineAgent");
        _interactiveObjectRecorder = GetComponent<InteractiveObjectRecorder>();
    }

    void Update()
    {
        if(m_IsActivated && m_CurrentPosition < m_PathToFollow.Length)
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
        if(!m_IsActivated)
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
        _interactiveObjectRecorder.ObjectInteraction(!_interactiveObjectRecorder.GetStatus());
    }

    public override void MoveObject()
    {
        m_IsActivated = true;
    }

    public override void ResetObject()
    {
        //TODO Write code to reset to original position
        Debug.LogError("MovableObject should have been reset to its original position, no code written for that yet");
    }
}