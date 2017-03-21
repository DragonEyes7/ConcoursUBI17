using UnityEngine;

public class EventObject : Interactive
{
    [SerializeField]Transform[] m_PathToFollow;
    [SerializeField]float _MovingSpeed = 0.003f;
    [SerializeField]int[] _EventTriggerID;
    [SerializeField]EventObject[] _EventObjects;

    Vector3[] _MovePositions;
    Quaternion[] _MoveRotations;

    int _NextPosition = 0;
    int _CurrentEvent = 0;

    float _StartMovingTime;
    float _DistanceMovingLength;
    Vector3 _StartMovingPosition;
    Quaternion _StartMovingRotation;

    InteractiveObjectRecorder _interactiveObjectRecorder;
    AudioSource _AudioSource;

    new void Start()
    {
        _AudioSource = GetComponent<AudioSource>();
        _interactiveObjectRecorder = GetComponent<InteractiveObjectRecorder>();
        _MovePositions = new Vector3[m_PathToFollow.Length + 1];
        _MoveRotations = new Quaternion[m_PathToFollow.Length + 1];
        _MovePositions[0] = transform.position;
        _MoveRotations[0] = transform.rotation;
        for (int i = 1; i < _MovePositions.Length; ++i)
        {
            _MovePositions[i] = m_PathToFollow[i - 1].position;
            _MoveRotations[i] = m_PathToFollow[i - 1].rotation;
        }        
        ++_NextPosition;
    }

    void Update () 
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
        transform.rotation = Quaternion.Slerp(_StartMovingRotation, _MoveRotations[_NextPosition], fracJourney);

        if (fracJourney >= 0.99f)
        {
            if (_CurrentEvent < _EventTriggerID.Length)
            {
                if (_NextPosition == _EventTriggerID[_CurrentEvent] + 1)
                {
                    _EventObjects[_CurrentEvent].Interact();
                    ++_CurrentEvent;
                }
            }
            ++_NextPosition;
            StartMoving();
            if (_NextPosition >= _MovePositions.Length)
            {
                return;
            }
        }
    }

    public override void ResetObject()
    {
        transform.position = _MovePositions[0];
        transform.rotation = _MoveRotations[0];
        m_IsActivated = false;
        _NextPosition = 1;
        _CurrentEvent = 0;
    }

    public override void Interact()
    {
        _interactiveObjectRecorder.ObjectInteraction(true);
    }

    public override void MoveObject()
    {
        StartMoving();
        m_IsActivated = true;
        _AudioSource.Play();
    }

    void StartMoving()
    {
        if (_NextPosition >= _MovePositions.Length) return;
        _StartMovingPosition = transform.position;
        _DistanceMovingLength = Vector3.Distance(_StartMovingPosition, _MovePositions[_NextPosition]);
        _StartMovingTime = Time.time;
    }
}
