using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]Camera m_Camera;
    [SerializeField]float m_ZoomLimit = 30f;
    [SerializeField]float m_ZoomSpeed = 0.01f;
    [SerializeField]float m_YMin = -40f;
    [SerializeField]float m_YMax = 50f;
    [SerializeField]float m_XMin;
    [SerializeField]float m_XMax;
    [SerializeField]bool m_YLock = true;
    [SerializeField]bool m_XLock = true;
    Vector2 m_Input;

    float m_CurrentX;
    float m_CurrentY;
    float m_MaxZoomOut;

    float _InitialFOV;

    AudioSource AS_Move, AS_Zoom, AS_ServoStop;

    bool _IsMoving = false, _IsZooming = false;

    void Start ()
    {
        AudioSource[] sounds = GetComponents<AudioSource>();
        AS_Move = sounds[0];
        AS_Zoom = sounds[1];
        AS_ServoStop = sounds[2];

        if (!m_Camera)
        {
            m_Camera = GetComponentInChildren<Camera>();
        }

        _InitialFOV = m_Camera.fieldOfView;
        ResetPosition();
	}
	
	void Update ()
    {
        m_Input.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Move();

        if (InputMode.isKeyboardMode)
        {
            Zoom(Input.GetAxis("Mouse ScrollWheel") * -m_ZoomSpeed);
        }
        else
        {
            Zoom(Input.GetAxis("Zoom") * m_ZoomSpeed);
        }        
    }

    void LateUpdate()
    {
        UpdatePosition();
    }

    void Move()
    {
        if (_IsMoving && m_Input.x == 0 && m_Input.y == 0)
        {
            AS_ServoStop.Play();
            AS_Move.Stop();
            _IsMoving = false;
        }
        else if (m_Input.x != 0 || m_Input.y != 0)
        {
            AS_Move.Play();
            AS_ServoStop.Stop();
            _IsMoving = true;
        }

        m_CurrentX += m_Input.x * m_Camera.fieldOfView / _InitialFOV;
        m_CurrentY += m_Input.y * m_Camera.fieldOfView / _InitialFOV;

        if (m_YLock)
        {
            m_CurrentY = Mathf.Clamp(m_CurrentY, m_YMin, m_YMax);
        }

        if(m_XLock)
        {
            m_CurrentX = Mathf.Clamp(m_CurrentX, m_XMin, m_XMax);
        }
    }

    void UpdatePosition()
    {
        transform.rotation = Quaternion.Euler(m_CurrentY, m_CurrentX, 0);
    }

    void Zoom(float zooming)
    {
        if (_IsZooming && zooming == 0.0f)
        {
            AS_Zoom.Stop();
            _IsZooming = false;
        }
        else if(zooming != 0.0f)
        {
            AS_Zoom.Play();
            _IsZooming = true;
        }

        m_Camera.fieldOfView += zooming;
        if (m_Camera.fieldOfView > m_MaxZoomOut)
        {
            m_Camera.fieldOfView = m_MaxZoomOut;
            AS_Zoom.Stop();
        }

        if (m_Camera.fieldOfView < m_ZoomLimit)
        {
            m_Camera.fieldOfView = m_ZoomLimit;
            AS_Zoom.Stop();
        }
    }

    public void SetLimitCam(float YMin, float YMax)
    {
        m_YMax = YMax;
        m_YMin = YMin;
    }

    public void SetupCam(float ZoomLimit, float ZoomSpeed, float YMin, float YMax, float XMin, float XMax, bool YLock, bool XLock)
    {
        m_ZoomLimit = ZoomLimit;
        m_ZoomSpeed = ZoomSpeed;
        m_YMin = YMin;
        m_YMax = YMax;
        m_XMin = XMin;
        m_XMax = XMax;
        m_YLock = YLock;
        m_XLock = XLock;
    }

    public void ResetPosition()
    {
        m_CurrentX = transform.localEulerAngles.y < m_XMin ? m_XMin : transform.localEulerAngles.y;
        m_CurrentY = transform.localEulerAngles.x < m_YMin ? m_YMin : transform.localEulerAngles.x;
        m_MaxZoomOut = m_Camera.fieldOfView;
        UpdatePosition();
    }
}