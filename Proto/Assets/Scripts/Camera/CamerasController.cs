using UnityEngine;
using System.Collections.Generic;

public class CamerasController : MonoBehaviour
{
    [SerializeField]List<GameObject> m_CameraObjects = new List<GameObject>();
    [SerializeField]Camera m_SceneCamera;
    PhotonView m_PhotonView;

    int m_LastCamera = 0;

    bool m_IsIntelligence = false;

    void Start ()
    {
        m_PhotonView = GetComponent<PhotonView>();
    }
	
	void Update ()
    {
		
	}

    public void SetIntelligence(bool value)
    {
        m_IsIntelligence = value;
        if (m_IsIntelligence)
        {
            m_SceneCamera.gameObject.SetActive(false);

            m_CameraObjects[0].GetComponent<PhotonView>().RequestOwnership();

            SetActiveCamera(0, m_LastCamera);           
        }
    }

    public void SetActiveCamera(int currentCam, int lastCam)
    {
        m_PhotonView.RPC("RPCActiveCamera", PhotonTargets.All, currentCam, lastCam);

        m_CameraObjects[lastCam].GetComponentInChildren<Camera>().enabled = false;
        m_CameraObjects[lastCam].GetComponent<CameraMovement>().enabled = false;
        m_CameraObjects[lastCam].GetComponentInChildren<IntelligenceAction>().enabled = false;
        
        m_CameraObjects[currentCam].GetComponentInChildren<Camera>().enabled = true;
        m_CameraObjects[currentCam].GetComponent<CameraMovement>().enabled = true;
        m_CameraObjects[currentCam].GetComponentInChildren<IntelligenceAction>().enabled = true;

        m_LastCamera = currentCam;
    }

    [PunRPC]
    void RPCActiveCamera(int currentCam, int lastCam)
    {
        m_CameraObjects[lastCam].GetComponent<Renderer>().material.color = Color.white;

        m_CameraObjects[currentCam].GetComponent<Renderer>().material.color = Color.red;
    }

    public void AddToCameraList(GameObject cameraToAdd)
    {
        if(m_IsIntelligence)
        {
            m_CameraObjects.Add(cameraToAdd);
            m_PhotonView.RPC("RPCAddCamera", PhotonTargets.Others, cameraToAdd.GetPhotonView().instantiationId);

            m_CameraObjects[m_CameraObjects.Count - 1].GetComponent<PhotonView>().RequestOwnership();

            TakeControl(cameraToAdd);
        }
    }

    [PunRPC]
    void RPCAddCamera(int id)
    {
        HackableCamera[] HCs = FindObjectsOfType<HackableCamera>();

        foreach (HackableCamera hc in HCs)
        {
            if(hc.gameObject.GetPhotonView().instantiationId == id)
            {
                m_CameraObjects.Add(hc.gameObject);
                return;
            }            
        }
    }

    public bool ContaintCamera(GameObject camera)
    {
        return m_CameraObjects.Contains(camera);
    }

    public void TakeControl(GameObject camera)
    {
        SetActiveCamera(m_CameraObjects.IndexOf(camera), m_LastCamera);
    }
}