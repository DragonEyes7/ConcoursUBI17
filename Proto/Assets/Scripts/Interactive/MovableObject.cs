using UnityEngine;

public class MovableObject : Interactive
{
    [SerializeField]Transform[] m_PathToFollow;
    [SerializeField]bool _MoveMoreThanOnce;
    [SerializeField]float _MovingSpeed = 0.003f;

    InteractiveObjectRecorder _interactiveObjectRecorder;
    Action _previousAction;
    AudioSource _AudioSource;

    Vector3[] _MovePositions;

    int m_CurrentPosition = 0;

    float _StartMovingTime;
    float _DistanceMovingLength = 1;
    Vector3 _StartMovingPosition;

    new void Start()
    {
        base.Start();
        _AudioSource = GetComponent<AudioSource>();
        m_SelectMat = Resources.Load<Material>("MAT_OutlineAgent");
        _interactiveObjectRecorder = GetComponent<InteractiveObjectRecorder>();
        _MovePositions = new Vector3[m_PathToFollow.Length + 1];
        _MovePositions[0] = transform.position;
        for (int i = 1; i < _MovePositions.Length; ++i)
        {
            _MovePositions[i] = m_PathToFollow[i - 1].position;
        }
    }

    void Update()
    {
        if(m_IsActivated && m_CurrentPosition < _MovePositions.Length)
        {
            Move();
        }
        else if(!m_IsActivated)
        {
            MoveBack();
        }
    }

    void Move()
    {
        float distCovered = (Time.time - _StartMovingTime) * _MovingSpeed;
        float fracJourney = distCovered / _DistanceMovingLength;
        transform.position = Vector3.Lerp(_StartMovingPosition, _MovePositions[m_CurrentPosition], fracJourney);

        if (fracJourney >= 0.99f)
        {
            ++m_CurrentPosition;
            if(m_CurrentPosition >= _MovePositions.Length)
            {
                m_CurrentPosition = _MovePositions.Length - 1;
                return;
            }
        }
    }

    void MoveBack()
    {
        float distCovered = (Time.time - _StartMovingTime) * _MovingSpeed;
        float fracJourney = distCovered / _DistanceMovingLength;
        transform.position = Vector3.Lerp(_StartMovingPosition, _MovePositions[m_CurrentPosition], fracJourney);

        if (fracJourney >= 0.99f)
        {
            --m_CurrentPosition;
            if (m_CurrentPosition < 0)
            {
                m_CurrentPosition = 0;
                m_IsActivated = false;
                return;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(!m_IsActivated || _MoveMoreThanOnce)
        {
            _previousAction = other.GetComponent<AgentActions>();
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
        _previousAction = other.GetComponent<AgentActions>();
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
        if (_AudioSource.isPlaying)
            return;
        _interactiveObjectRecorder.ObjectInteraction(!_interactiveObjectRecorder.GetStatus(), _MoveMoreThanOnce);
    }

    public override void MoveObject()
    {
        m_IsActivated = !m_IsActivated;
        _StartMovingTime = Time.time;
        _StartMovingPosition = transform.position;
        _DistanceMovingLength = Vector3.Distance(_MovePositions[m_CurrentPosition], transform.position);
        _AudioSource.Play();
    }

    public override void ResetObject()
    {
        transform.position = _MovePositions[0];
        m_IsActivated = false;
        m_CurrentPosition = 0;
    }
}