using UnityEngine;

public class Detective : MonoBehaviour
{
    [SerializeField]Camera[] m_Cameras;
    [SerializeField]Camera m_SceneCamera;

    int m_CurrentCamera = 0;

	void Start ()
    {
        m_SceneCamera.gameObject.SetActive(false);
        m_Cameras[m_CurrentCamera].gameObject.SetActive(true);
	}
	
	void Update ()
    {
		if(Input.GetButtonDown("PrevCamera"))
        {
            --m_CurrentCamera;
            if(m_CurrentCamera < 0)
            {
                m_CurrentCamera = m_Cameras.Length - 1;
            }
            RefreshCamera();
        }

        if (Input.GetButtonDown("NextCamera"))
        {
            ++m_CurrentCamera;
            if (m_CurrentCamera >= m_Cameras.Length)
            {
                m_CurrentCamera = 0;
            }
            RefreshCamera();
        }
    }

    void RefreshCamera()
    {
        for(int i = 0; i < m_Cameras.Length; ++i)
        {
            m_Cameras[i].gameObject.SetActive(false);
        }
        m_Cameras[m_CurrentCamera].gameObject.SetActive(true);
    }
}