using UnityEngine;

public class CameraSetup : MonoBehaviour
{
    [SerializeField] float m_ZoomLimit = 30f;
    [SerializeField] float m_ZoomSpeed = 0.01f;
    [SerializeField] float m_YMin = -40f;
    [SerializeField] float m_YMax = 50f;
    [SerializeField] float m_XMin;
    [SerializeField] float m_XMax;
    [SerializeField] bool m_YLock = true;
    [SerializeField] bool m_XLock = true;

    public void SetupCamera(CameraMovement script)
    {
        script.SetupCam(m_ZoomLimit, m_ZoomSpeed, m_YMin, m_YMax, m_XMin, m_XMax, m_YLock, m_XLock);
    }
}
