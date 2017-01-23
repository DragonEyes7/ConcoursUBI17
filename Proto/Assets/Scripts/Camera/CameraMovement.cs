using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Vector2 m_Input;

    float m_CurrentYMin = -40f;
    float m_CurrentYMax = 50f;
    float m_CurrentXMin;
    float m_CurrentXMax;

    float m_CurrentX;
    float m_CurrentY;

    void Start ()
    {
        m_CurrentX = transform.rotation.eulerAngles.x;
        m_CurrentY = transform.rotation.eulerAngles.y;
	}
	
	void Update ()
    {
        m_Input.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if(m_Input.magnitude > 0)
        {
            Move();
        }        
    }

    void LateUpdate()
    {
        UpdatePosition();
    }

    void Move()
    {
        m_CurrentX += m_Input.x;
        m_CurrentY += m_Input.y * -1;

        m_CurrentY = Mathf.Clamp(m_CurrentY, m_CurrentYMin, m_CurrentYMax);
    }

    void UpdatePosition()
    {
        transform.rotation = Quaternion.Euler(m_CurrentY, m_CurrentX, 0);
    }

    public void SetLimitCam(float YMin, float YMax)
    {
        m_CurrentYMax = YMax;
        m_CurrentYMin = YMin;
    }
}