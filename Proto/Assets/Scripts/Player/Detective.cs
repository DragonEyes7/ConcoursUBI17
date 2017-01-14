using UnityEngine;

public class Detective : MonoBehaviour
{
    [SerializeField]Camera[] m_Cameras;
    [SerializeField]Camera m_SceneCamera;

    CamerasController m_CamerasController;

    int[] m_TargetCharacteristics = new int[6];

    int m_CurrentCamera = 0;

	void Start ()
    {
        m_SceneCamera.gameObject.SetActive(false);

        m_CamerasController = GetComponent<CamerasController>();

        RefreshCamera();       

        GiveSuspectCharacteristics();
    }

    void GiveSuspectCharacteristics()
    {
        Characteristics[] targets = FindObjectsOfType<Characteristics>();

        if (targets.Length > 0)
        {
            Characteristics target = targets[Random.Range(0, targets.Length)];
            if (target)
            {
                m_TargetCharacteristics = target.GetCharacteristics();
            }
        }
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

        if(Input.GetButtonDown("Action"))
        {
            Action();
        }
    }

    void RefreshCamera()
    {
        for(int i = 0; i < m_Cameras.Length; ++i)
        {
            m_Cameras[i].gameObject.SetActive(false);
        }
        m_Cameras[m_CurrentCamera].gameObject.SetActive(true);
        m_CamerasController.SetActiveCamera(m_CurrentCamera);
    }

    void Action()
    {
        //if blabla
        InspectTarget();
    }

    void InspectTarget()
    {
        Characteristics[] targets = FindObjectsOfType<Characteristics>();

        if(targets.Length > 0)
        {
            Characteristics target = targets[Random.Range(0, targets.Length)];
            if(target)
            {
                int[] characteristics = target.GetCharacteristics();

                for(int i = 0; i < characteristics.Length; ++i)
                {
                    if(characteristics[i] != m_TargetCharacteristics[i])
                    {
                        Debug.Log("Not the Target");
                        return;
                    }
                }

                Debug.Log("Target Found: " + target.gameObject.name);
                return;
            }
        }
    }
}