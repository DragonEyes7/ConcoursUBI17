using UnityEngine;

public class Movement : MonoBehaviour
{
    Vector2 m_Input;
    Vector3 m_MoveVector;

    void Start ()
    {
		
	}
	
	void Update ()
    {
        m_Input.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (m_Input.magnitude > 0)
        {
            Move();
        }
    }

    void Move()
    {
        float controllerSpeed = m_Input.magnitude;

        if (controllerSpeed > 1f)
        {
            controllerSpeed = 1f;
        }

        m_MoveVector = new Vector3(m_Input.x, 0, m_Input.y) * controllerSpeed * Time.deltaTime;

        transform.position = transform.position + m_MoveVector;
    }
}