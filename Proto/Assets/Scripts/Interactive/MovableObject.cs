using UnityEngine;

public class MovableObject : Interactive
{
    [SerializeField]protected Transform[] m_PathToFollow;
    [SerializeField]protected bool _MoveMoreThanOnce;
    [SerializeField]protected float _MovingSpeed = 0.003f;

    protected InteractiveObjectRecorder _interactiveObjectRecorder;
    protected Action _previousAction;
    protected AudioSource _AudioSource;

    protected Vector3[] _MovePositions;

    protected int _NextPosition = 0;

    protected float _StartMovingTime;
    protected float _DistanceMovingLength;
    protected Vector3 _StartMovingPosition;
    protected PhotonView _PhotonView;

    protected new void Start()
    {
        base.Start();
        _PhotonView = GetComponent<PhotonView>();
        _AudioSource = GetComponent<AudioSource>();
        m_SelectMat = Resources.Load<Material>("MAT_OutlineAgent");
        _interactiveObjectRecorder = GetComponent<InteractiveObjectRecorder>();
        _MovePositions = new Vector3[m_PathToFollow.Length + 1];
        _MovePositions[0] = transform.position;
        for (int i = 1; i < _MovePositions.Length; ++i)
        {
            _MovePositions[i] = m_PathToFollow[i - 1].position;
        }
        ++_NextPosition;
    }

    protected void Update()
    {
        if (m_IsActivated && _NextPosition < _MovePositions.Length)
        {
            Move();
        }
    }

    void Move()
    {
        float distCovered = (Time.time - _StartMovingTime) * _MovingSpeed;
        float fracJourney = distCovered / _DistanceMovingLength;
        transform.position = Vector3.Lerp(_StartMovingPosition, _MovePositions[_NextPosition], fracJourney);

        if (fracJourney >= 0.99f)
        {
            ++_NextPosition;
            StartMoving();
            if (_NextPosition >= _MovePositions.Length)
            {
                _NextPosition = _MoveMoreThanOnce ? _MovePositions.Length - 1 : _MovePositions.Length;
                return;
            }
        }
    }

    void MoveBack()
    {
        float distCovered = (Time.time - _StartMovingTime) * _MovingSpeed;
        float fracJourney = distCovered / _DistanceMovingLength;
        transform.position = Vector3.Lerp(_StartMovingPosition, _MovePositions[_NextPosition], fracJourney);

        if (fracJourney >= 0.99f)
        {
            --_NextPosition;
            StartMoving();
            if (_NextPosition < 0)
            {
                _NextPosition = 0;
                m_IsActivated = false;
                return;
            }
        }
    }

    protected void OnTriggerStay(Collider other)
    {
        if (!m_IsActivated || _MoveMoreThanOnce)
        {
            _previousAction = other.GetComponent<AgentActions>();
            if (_previousAction)
            {
                if (_previousAction.enabled)
                {
                    m_HUD.ShowActionPrompt("Move Object");
                    _previousAction.SetInteract(true);
                    _previousAction.SetInteractionObject(this);
                    Select();
                }
            }
        }
        else if (m_IsActivated && !_MoveMoreThanOnce)
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
    }

    protected void OnTriggerExit(Collider other)
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
        _interactiveObjectRecorder.ObjectInteraction(true, _MoveMoreThanOnce);
    }

    public override void MoveObject()
    {
        //transform.position = _MovePositions[_MovePositions.Length - 1];
        //m_CurrentPosition = _MovePositions.Length;  
        //m_IsActivated = false;
        if (_AudioSource.isPlaying)
            return;

        StartMoving();
        _AudioSource.Play();
        m_IsActivated = true;
    }

    public override void ResetObject()
    {
        transform.position = _MovePositions[0];
        m_IsActivated = false;
        _NextPosition = 1;
    }

    void StartMoving()
    {
        if (_NextPosition >= _MovePositions.Length) return;
        _StartMovingPosition = transform.position;
        _DistanceMovingLength = Vector3.Distance(_StartMovingPosition, _MovePositions[_NextPosition]);
        _StartMovingTime = Time.time;
    }
}