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

    void Start ()
    {
        if(!m_Camera)
        {
            m_Camera = GetComponentInChildren<Camera>();
        }
        
        ResetPosition();
	}
	
	void Update ()
    {
        m_Input.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if(m_Input.magnitude > 0)
        {
            Move();
        }

        if(InputMode.isKeyboardMode)
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
        m_CurrentX += m_Input.x;
        m_CurrentY += m_Input.y;

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
        m_Camera.fieldOfView += zooming;
        if (m_Camera.fieldOfView > m_MaxZoomOut)
        {
            m_Camera.fieldOfView = m_MaxZoomOut;
        }

        if (m_Camera.fieldOfView < m_ZoomLimit)
        {
            m_Camera.fieldOfView = m_ZoomLimit;
        }
    }

    public void SetLimitCam(float YMin, float YMax)
    {
        m_YMax = YMax;
        m_YMin = YMin;
    }

    public void ResetPosition()
    {
        m_CurrentX = transform.localEulerAngles.y < m_XMin ? m_XMin : transform.localEulerAngles.y;
        m_CurrentY = transform.localEulerAngles.x < m_YMin ? m_YMin : transform.localEulerAngles.x;
        m_MaxZoomOut = m_Camera.fieldOfView;
        UpdatePosition();
    }
}