using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]Transform m_CameraTransform;
    [SerializeField]float m_ZoomLimit = 4f;
    [SerializeField]float m_YMin = -40f;
    [SerializeField]float m_YMax = 50f;
    [SerializeField]float m_XMin;
    [SerializeField]float m_XMax;
    Vector2 m_Input;

    float m_CurrentX;
    float m_CurrentY;

    void Start ()
    {
        m_CurrentX = transform.localEulerAngles.x;
        m_CurrentY = transform.localEulerAngles.y;
	}
	
	void Update ()
    {
        m_Input.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if(m_Input.magnitude > 0)
        {
            Move();
        }

        Zoom(Input.GetAxis("Mouse ScrollWheel") * -1f);
    }

    void LateUpdate()
    {
        UpdatePosition();
    }

    void Move()
    {
        m_CurrentX += m_Input.x;
        m_CurrentY += m_Input.y;

        m_CurrentY = Mathf.Clamp(m_CurrentY, m_YMin, m_YMax);
        m_CurrentX = Mathf.Clamp(m_CurrentX, m_XMin, m_XMax);
    }

    void UpdatePosition()
    {
        transform.rotation = Quaternion.Euler(m_CurrentY, m_CurrentX, 0);
    }

    void Zoom(float zooming)
    {
        m_CameraTransform.localPosition += new Vector3(0, 0, zooming);
        if (m_CameraTransform.localPosition.z > 0)
        {
            m_CameraTransform.localPosition = Vector3.zero;
        }

        if (m_CameraTransform.localPosition.z < m_ZoomLimit*-1f)
        {
            m_CameraTransform.localPosition = new Vector3(0,0,m_ZoomLimit*-1f);
        }
    }

    public void SetLimitCam(float YMin, float YMax)
    {
        m_YMax = YMax;
        m_YMin = YMin;
    }
}