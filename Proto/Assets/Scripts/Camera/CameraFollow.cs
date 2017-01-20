﻿using UnityEngine;
using System.Collections.Generic;

public class CameraFollow : MonoBehaviour
{
	[SerializeField] LayerMask m_LimitLayer;
    [SerializeField] float m_LimitUnderGroundRayLength = 3f;
    [SerializeField] float m_LimitBottomRayLength = 0.3f;
	[SerializeField] float m_LimitSidesRayLength = 0.3f;
	[SerializeField] float m_MinAngleY = -50f;
    [SerializeField] float m_MaxAngleY = 75f;
	//[SerializeField] float m_ResetDelay = 0.25f;
    [SerializeField] float m_MinDistance = 2f;
    [SerializeField] float m_MaxDistance = 7f;
    [SerializeField] float m_ZoomSpeed = 0.01f;
    [SerializeField] float m_SmoothZoomSpeed = 0.03f;

	Transform m_Player;
	
    List<Transform> m_Transparent = new List<Transform>();
	List<Shader> m_Shaders = new List<Shader>();

	float m_CurrentYMin;
	float m_CurrentYMax;

	float m_CurrentX;
    float m_CurrentY;

	//float m_ResetTimer = 0;

	Vector3 m_Distance;
	Vector3 m_LookAt;
    float m_LastDistance;

    bool m_SmoothZoom;
    bool m_Control = true;

	void Start()
	{
		m_Distance = new Vector3();
		m_LookAt = new Vector3();

		m_CurrentYMin = m_MinAngleY;
		m_CurrentYMax = m_MaxAngleY;
    }

	void Update()
    {
		if(m_Player)
		{
			#region Debug
			Debug.DrawRay(transform.position, Vector3.down * m_LimitBottomRayLength, Color.green);
            Debug.DrawRay(transform.position, Vector3.up * m_LimitUnderGroundRayLength, Color.green);
            Debug.DrawRay(transform.position, Vector3.left * m_LimitSidesRayLength, Color.blue);
			Debug.DrawRay(transform.position, Vector3.right * m_LimitSidesRayLength, Color.blue);
			#endregion

			#region Collision Detection

			/*if (Time.time >= m_ResetTimer )
			{
				m_CurrentYMin = m_MinAngleY;
			}*/

			HitFloor();
			#endregion

			#region Camera movement
            if(m_Control)
            {
                if (InputMode.isKeyboardMode)
                {
                    if (Input.GetMouseButton(1))
                    {
                        Cursor.lockState = CursorLockMode.Locked;
                        Cursor.visible = false;

                        Move(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0);
                    }

                    if (Input.GetMouseButtonUp(1))
                    {
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                    }
                }
                else
                {
                    Move(Input.GetAxis("CameraHorizontal"), Input.GetAxis("CameraVertical"), 1);
                }
            }
			#endregion

			#region Camera Zooming
            if(m_SmoothZoom)
            {
                PlayerSettings.CameraDistance += m_SmoothZoomSpeed;

                if (PlayerSettings.CameraDistance >= m_LastDistance)
                {
                    m_SmoothZoom = false;
                    PlayerSettings.CameraDistance = m_LastDistance;
                    m_LastDistance = 0;
                }
            }

			if (Input.GetKey(KeyCode.Z))
			{
				if (PlayerSettings.CameraDistance < m_MaxDistance)
				{
					PlayerSettings.CameraDistance += m_ZoomSpeed;
				}
			}

			if (Input.GetKey(KeyCode.X))
			{
				if (PlayerSettings.CameraDistance > m_MinDistance)
				{
					PlayerSettings.CameraDistance -= m_ZoomSpeed;
				}
			}
			#endregion

			#region Visible obstacle
			//Set Object visible or not.
			RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, Vector3.Distance(m_Player.transform.position, transform.position));
			Debug.DrawLine(transform.position, m_Player.transform.position, Color.red);
			bool toDelete = true;

			for (int i = 0; i < hits.Length; i++)
			{
				if (hits[i].transform.GetInstanceID() != m_Player.GetInstanceID() && hits[i].transform.tag != "LevelLimit")
				{
					MeshRenderer rend = hits[i].transform.GetComponent<MeshRenderer>();

					if (rend && !m_Transparent.Contains(hits[i].transform))
					{
						m_Transparent.Add(hits[i].transform);
						m_Shaders.Add(rend.material.shader);
						rend.material.shader = Shader.Find("Custom/TransparentShader");
					}
				}
                else if(hits[i].transform.tag == "LevelLimit")
                {
                    UnderGround();
                }
			}

			for (int i = 0; i < m_Transparent.ToArray().Length; ++i)
			{
				foreach (RaycastHit hit in hits)
				{
					if (m_Transparent[i] == hit.transform)
					{
						toDelete = false;
						break;
					}
				}

				if (toDelete && m_Transparent[i])
				{
					Renderer rend = m_Transparent[i].GetComponent<Renderer>();
					if(rend)
					{
						rend.material.shader = m_Shaders[i];

						m_Transparent.Remove(m_Transparent[i]);
						m_Shaders.Remove(m_Shaders[i]);
					}
				}
			}
			#endregion
		}
	}

	void LateUpdate()
	{
        UpdatePosition();
    }

    public void SetPlayer(Transform player)
    {
        m_Player = player;
    }

	void HitFloor()
	{
		RaycastHit hit;
		if (Physics.Raycast(transform.position, Vector3.down, out hit, m_LimitBottomRayLength, m_LimitLayer))
		{
			//m_ResetTimer = Time.time + m_ResetDelay;
            m_CurrentYMin = m_CurrentY;// + m_LimitBottomRayLength - 0.1f;
		}
        else
        {
            m_CurrentYMin = m_MinAngleY;
        }
	}

    bool isHittingFloor()
    {
        return Physics.Raycast(transform.position, Vector3.down, m_LimitBottomRayLength, m_LimitLayer);
    }

    void UnderGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.up, out hit, m_LimitUnderGroundRayLength, m_LimitLayer))
        {
            m_CurrentY += Vector3.Angle(transform.position, hit.point) + 0.5f;
            m_CurrentYMin = m_CurrentY;
            UpdatePosition();
        }
    }

	bool HitLimitLeft()
	{
		return Physics.Raycast(transform.position, Vector3.left, m_LimitSidesRayLength, m_LimitLayer);
	}

	bool HitLimitRight()
	{
		return Physics.Raycast(transform.position, Vector3.right, m_LimitSidesRayLength, m_LimitLayer);
	}

	void Move(float X, float Y, int InputType)
	{
		m_CurrentX += X * PlayerSettings.GetCameraSpeed(InputType);
        m_CurrentY += Y * PlayerSettings.GetCameraSpeed(InputType) * -1;

        m_CurrentY = Mathf.Clamp(m_CurrentY, m_CurrentYMin, m_CurrentYMax);

        if (m_CurrentY <= m_CurrentYMin)
        {
            if(m_LastDistance == 0)
            {
                m_LastDistance = PlayerSettings.CameraDistance;
            }

            if (PlayerSettings.CameraDistance > m_MinDistance)
            {
                PlayerSettings.CameraDistance -= m_ZoomSpeed * Y;
            }
        }
        else if(m_LastDistance > 0 && Y < 0)
        {
            m_CurrentYMin = m_MinAngleY;
            m_SmoothZoom = true;
        }
	}

	void UpdatePosition()
	{
        if(m_Player)
        {
            m_Distance.Set(0, 0, -PlayerSettings.CameraDistance);
            Quaternion rotation = Quaternion.Euler(m_CurrentY, m_CurrentX, 0);

            transform.position = m_Player.position + rotation * m_Distance;

            m_LookAt.Set(m_Player.position.x, m_Player.position.y, m_Player.position.z);
            transform.LookAt(m_LookAt);
        }
    }

    public void ResetPosition()
    {
        m_CurrentX = m_Player.eulerAngles.y;
    }
}