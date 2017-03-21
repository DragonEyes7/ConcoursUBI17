using UnityEngine;

public class MovableEventObject : MovableObject
{
    [Header("EventObject")]
    [SerializeField]int[] _EventTriggerID;
    [SerializeField]EventObject[] _EventObjects;

    Quaternion[] _MoveRotations;
    int _CurrentEvent;
    private readonly Quaternion _startMovingRotation = new Quaternion(0,0,0,0);

    new void Start ()
	{
        base.Start();
        _AudioSource = GetComponent<AudioSource>();
        m_SelectMat = Resources.Load<Material>("MAT_OutlineAgent");
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

    new void Update()
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
        transform.rotation = Quaternion.Slerp(_startMovingRotation, _MoveRotations[_NextPosition], fracJourney);

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

    public override void ResetObject()
    {
        transform.position = _MovePositions[0];
        transform.rotation = _MoveRotations[0];
        m_IsActivated = false;
        _NextPosition = 1;
        _CurrentEvent = 0;
    }

    void StartMoving()
    {
        if (_NextPosition >= _MovePositions.Length) return;
        _StartMovingPosition = transform.position;
        _DistanceMovingLength = Vector3.Distance(_StartMovingPosition, _MovePositions[_NextPosition]);
        _StartMovingTime = Time.time;
    }
}
