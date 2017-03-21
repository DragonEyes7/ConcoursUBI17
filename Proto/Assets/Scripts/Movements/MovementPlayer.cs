using UnityEngine;

public class MovementPlayer : Movement
{
    [SerializeField]float m_TurnSpeed = 6f;
    [SerializeField]float m_SpeedWalking = 3f;
    Transform m_CameraTransform;
    Rigidbody m_Rigidbody;
    Vector2 m_Input;
    Vector3 m_MoveVector;
    float m_SpeedCurrent;

    Animator _Animator;

    bool _IsPaused = true;
    bool _IdleBreak = false;
    bool _CanMove = true;

    public bool isPaused
    {
        get { return _IsPaused; }
        set { _IsPaused = value; }
    }

    new void Start ()
    {
        base.Start();
        _Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_SpeedCurrent = m_SpeedWalking;
    }
	
	void FixedUpdate()
    {
        if(m_Recorder.IsRecording && _CanMove)
        {
            m_Input.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (m_Input.magnitude > 0)
            {
                Move();
            }
            else
            {
                _Animator.SetFloat("Speed", 0);

                if(!_IdleBreak)
                {
                    _IdleBreak = true;
                    Invoke("IdleBreak", Random.Range(2, 5));
                }
            }
        }
    }

    Vector3 InputDirection()
    {
        if (!m_CameraTransform)
        {
            m_CameraTransform = GetComponent<PlayerSetup>().GetPlayerCamera();
            if (!m_CameraTransform)
            {
                return Vector3.zero;
            }
        }

        Vector3 storeDir = m_CameraTransform.right;
        Vector3 direct = transform.position + (storeDir * m_Input.x) + m_CameraTransform.forward * m_Input.y;
        Vector3 dir = direct - transform.position;
        dir.y = 0;

        return dir;
    }

    void Move()
    {
        Vector3 dir = InputDirection();

        float controllerSpeed = m_Input.magnitude;
        float angle;

        IdleBreakReset();
        if (controllerSpeed > 1f)
        {
            controllerSpeed = 1f;
        }
        else if (controllerSpeed < 0.4f && controllerSpeed > 0)
        {
            angle = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(dir));

            if (angle != 0)
            {
                m_Rigidbody.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), m_TurnSpeed * Time.deltaTime);
            }         
               
            return;
        }

        _Animator.SetFloat("Speed", controllerSpeed);

        m_MoveVector = transform.forward.normalized * m_SpeedCurrent * controllerSpeed * Time.deltaTime;

        m_Rigidbody.MovePosition(transform.position + m_MoveVector);

        angle = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(dir));

        if (angle != 0)
        {
            m_Rigidbody.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), m_TurnSpeed * Time.deltaTime);
        }
    }

    void IdleBreak()
    {
        _Animator.SetTrigger("IdleBreak");
        Invoke("IdleBreakReset", Random.Range(10, 15));
    }

    void IdleBreakReset()
    {
        CancelInvoke("IdleBreak");
        _IdleBreak = false;
    }

    public void CantMove()
    {
        _CanMove = false;
    }

    public void CanMove()
    {
        GetComponent<Animator>().SetBool("Interact", false);
        _CanMove = true;
    }
}