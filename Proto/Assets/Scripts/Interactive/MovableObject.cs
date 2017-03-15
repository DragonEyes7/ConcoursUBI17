using UnityEngine;

public class MovableObject : Interactive
{
    [SerializeField]Transform[] m_PathToFollow;

    InteractiveObjectRecorder _interactiveObjectRecorder;
    Vector3 _startPosition;
    Action _previousAction;
    AudioSource _AudioSource;

    int m_CurrentPosition = 0;

    new void Start()
    {
        base.Start();
        _AudioSource = GetComponent<AudioSource>();
        m_SelectMat = Resources.Load<Material>("MAT_OutlineAgent");
        _interactiveObjectRecorder = GetComponent<InteractiveObjectRecorder>();
        _startPosition = transform.position;
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
            _previousAction = other.GetComponent<Action>();
            if (_previousAction)
            {
                if (_previousAction.enabled)
                {
                    m_HUD.ShowActionPrompt("Move Chair");
                    _previousAction.SetInteract(true);
                    _previousAction.SetInteractionObject(this);
                    Select();
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        _previousAction = other.GetComponent<Action>();
        if (_previousAction)
        {
            if (_previousAction.enabled)
            {
                m_HUD.HideActionPrompt();
                _previousAction.SetInteract(false);
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
        _AudioSource.Play();
    }

    public override void ResetObject()
    {
        transform.position = _startPosition;
        m_IsActivated = false;
        m_CurrentPosition = 0;

        if (_previousAction)
        {
            _previousAction.SetInteract(true);
        }
    }

    public void SetPathToFollow(Transform[] path)
    {
        m_PathToFollow = path;
    }
}