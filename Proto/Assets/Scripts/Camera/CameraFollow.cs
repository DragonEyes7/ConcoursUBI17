using UnityEngine;
using System;
using System.Collections.Generic;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]LayerMask m_LimitLayer;
    [SerializeField]float m_LimitUnderGroundRayLength = 3f;
    [SerializeField]float m_LimitBottomRayLength = 0.3f;
    [SerializeField]float m_LimitSidesRayLength = 0.3f;
    [SerializeField]float m_MinAngleY = -50f;
    [SerializeField]float m_MaxAngleY = 75f;
    [SerializeField]float m_MinDistance = 2f;
    [SerializeField]float m_MaxDistance = 7f;
    [SerializeField]float m_ZoomSpeed = 0.01f;
    [SerializeField]float m_MinZoomFOV = 60f;
    [SerializeField]float m_MaxZoomFOV = 90f;
    [SerializeField]float m_SmoothZoomSpeed = 0.03f;

    Transform m_PlayerFollow;
    Camera m_Camera;

    List<Transform> m_Transparent = new List<Transform>();
    List<Shader> m_Shaders = new List<Shader>();

    Dictionary<int, List<Shader>> _ChildrensShaders = new Dictionary<int, List<Shader>>();

    float m_CurrentYMin;
    float m_CurrentYMax;

    float m_CurrentX;
    float m_CurrentY;

    Vector3 m_Distance;
    Vector3 m_LookAt;
    float m_LastDistance;
    public float m_CurrentDistance;

    float m_MinFOV = 60f;
    float m_MaxFOV = 65f;    

    bool m_SmoothZoom;
    bool m_Control = true;
    Shader _ARCShader;

    #region Debug
    Vector3 m_LastPosition;
    #endregion

    void Start()
    {
        m_Camera = GetComponent<Camera>();
        m_Camera.fieldOfView = m_MinFOV;

        m_Distance = new Vector3();
        m_LookAt = new Vector3();

        m_CurrentYMin = m_MinAngleY;
        m_CurrentYMax = m_MaxAngleY;

        m_CurrentDistance = PlayerSettings.CameraDistance;

        SetPlayerMaxDistancePref();

        m_LastPosition = transform.position;
        _ARCShader = Shader.Find("Custom/ARC");
    }

    void Update()
    {
        if (m_PlayerFollow)
        {
            #region Debug
            Debug.DrawRay(transform.position, Vector3.down * m_LimitBottomRayLength, Color.green);
            Debug.DrawRay(transform.position, Vector3.up * m_LimitUnderGroundRayLength, Color.green);
            Debug.DrawRay(transform.position, Vector3.left * m_LimitSidesRayLength, Color.blue);
            Debug.DrawRay(transform.position, Vector3.right * m_LimitSidesRayLength, Color.blue);
            if(m_LastPosition != transform.position)
            {
                Debug.DrawLine(transform.position, m_LastPosition, Color.red, 10f);
            }
            
            m_LastPosition = transform.position;
            #endregion

            #region Collision Detection

            #endregion

            #region Camera movement
            if (m_Control)
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
            if (m_SmoothZoom && !isHittingFloor())
            {
                m_CurrentDistance += m_SmoothZoomSpeed;
                float currentDistance = ((m_MaxDistance - m_MinDistance) * (m_CurrentY + Math.Abs(m_CurrentYMin)) / (m_CurrentYMax + Math.Abs(m_CurrentYMin))) + m_MinDistance;
                if (m_CurrentDistance >= currentDistance)
                {
                    m_SmoothZoom = false;
                    m_CurrentDistance = currentDistance;
                    m_LastDistance = 0;
                }
            }

            if (Input.GetAxisRaw("DPadY") != 0 && InputMode.isInMenu)
            {
                m_Camera.fieldOfView -= Input.GetAxisRaw("DPadY") * m_ZoomSpeed;
                if (m_Camera.fieldOfView < m_MinZoomFOV)
                {
                    m_Camera.fieldOfView = m_MinZoomFOV;
                }

                if (m_Camera.fieldOfView > m_MaxZoomFOV)
                {
                    m_Camera.fieldOfView = m_MaxZoomFOV;
                }
            }
            #endregion

            #region Visible obstacle
            RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, Vector3.Distance(m_PlayerFollow.transform.position, transform.position));
            Debug.DrawLine(transform.position, m_PlayerFollow.transform.position, Color.red);
            bool toDelete = true;

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform.GetInstanceID() != m_PlayerFollow.GetInstanceID() && hits[i].transform.tag != "LevelLimit")
                {
                    MeshRenderer rend = hits[i].transform.GetComponent<MeshRenderer>();

                    if (rend && !m_Transparent.Contains(hits[i].transform))
                    {
                        m_Transparent.Add(hits[i].transform);
                        m_Shaders.Add(rend.material.shader);

                        MeshRenderer[] renders = hits[i].transform.GetComponentsInChildren<MeshRenderer>();
                        List<Shader> shaders = new List<Shader>();
                        foreach (MeshRenderer render in renders)
                        {
                            shaders.Add(render.material.shader);
                            render.material.shader = _ARCShader;
                        }

                        _ChildrensShaders.Add(hits[i].transform.GetInstanceID(), shaders);

                        rend.material.shader = _ARCShader;
                    }
                }
                else if (hits[i].transform.tag == "LevelLimit")
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
                    if (rend)
                    {
                        rend.material.shader = m_Shaders[i];
                        if(_ChildrensShaders.ContainsKey(m_Transparent[i].GetInstanceID()))
                        {
                            MeshRenderer[] renders = m_Transparent[i].transform.GetComponentsInChildren<MeshRenderer>();

                            List<Shader> shaders = _ChildrensShaders[m_Transparent[i].transform.GetInstanceID()];
                            for (int j = 0; j < shaders.Count; ++j)
                            {
                                renders[j].material.shader = shaders[j];
                            }
                            _ChildrensShaders.Remove(m_Transparent[i].transform.GetInstanceID());
                        }

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
        m_PlayerFollow = player;
    }

    void HitFloor()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, m_LimitBottomRayLength, m_LimitLayer))
        {
            m_CurrentYMin = m_CurrentY;
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
        m_CurrentX += X * PlayerSettings.GetCameraSpeed(InputType) * Time.deltaTime;

        if (!isHittingFloor() || Y < 0)
        {
            m_CurrentY += Y * PlayerSettings.GetCameraSpeed(InputType) * -1 * Time.deltaTime;
        }

        m_CurrentY = Mathf.Clamp(m_CurrentY, m_CurrentYMin, m_CurrentYMax);
        
        m_Camera.fieldOfView = m_MaxFOV - ((m_MaxFOV-m_MinFOV) * (m_CurrentY + Math.Abs(m_CurrentYMin)) / (m_CurrentYMax + Math.Abs(m_CurrentYMin)));

        if(isHittingFloor() && Y > 0 && m_LastDistance == 0)
        {
            m_LastDistance = m_CurrentDistance;
        }

        if(m_LastDistance != 0 && Y < 0 && !m_SmoothZoom)
        {
            m_SmoothZoom = true;
        }
        else if(m_LastDistance != 0 && !m_SmoothZoom)
        {
            m_CurrentDistance -= m_ZoomSpeed * Y;
            if(m_CurrentDistance < m_MinDistance)
            {
                m_CurrentDistance = m_MinDistance;
            }
        }
        else if(m_LastDistance == 0)
        {
            m_CurrentDistance = ((m_MaxDistance - m_MinDistance) * (m_CurrentY + Math.Abs(m_CurrentYMin)) / (m_CurrentYMax + Math.Abs(m_CurrentYMin))) + m_MinDistance;
        }
    }

    void UpdatePosition()
    {
        if (m_PlayerFollow)
        {
            m_Distance.Set(0, 0, -m_CurrentDistance);
            Quaternion rotation = Quaternion.Euler(m_CurrentY, m_CurrentX, 0);

            transform.position = m_PlayerFollow.position + rotation * m_Distance;

            m_LookAt.Set(m_PlayerFollow.position.x, m_PlayerFollow.position.y, m_PlayerFollow.position.z);
            transform.LookAt(m_LookAt);
        }
    }

    public void ResetPosition()
    {
        m_CurrentX = m_PlayerFollow.eulerAngles.y;
    }

    public void SetPlayerMaxDistancePref()
    {
        m_MaxDistance = PlayerSettings.CameraDistance;
    }
}