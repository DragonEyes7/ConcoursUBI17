using UnityEngine;
using System.Collections.Generic;

public class CamerasController : MonoBehaviour
{
    [SerializeField]GameObject[] m_CameraObjects;
    [SerializeField]int m_NumberOfStartingCamera = 1;
    [SerializeField]Camera m_SceneCamera;
    PhotonView m_PhotonView;
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
        if(value)
        {
            m_SceneCamera.gameObject.SetActive(false);
            SetActiveCamera(0, 0);
            m_CameraObjects[0].GetComponent<PhotonView>().RequestOwnership();
            m_CameraObjects[0].GetComponentInChildren<Camera>().enabled = true;
            m_CameraObjects[0].GetComponent<CameraMovement>().enabled = true;
            m_CameraObjects[0].GetComponentInChildren<IntelligenceAction>().SetActive(true);
        }
    }

    public void SetActiveCamera(int currentCam, int lastCam)
    {
        m_PhotonView.RPC("RPCActiveCamera", PhotonTargets.All, currentCam, lastCam);
    }

    [PunRPC]
    void RPCActiveCamera(int currentCam, int lastCam)
    {
        m_CameraObjects[lastCam].GetComponent<Renderer>().material.color = Color.white;

        m_CameraObjects[currentCam].GetComponent<Renderer>().material.color = Color.red;
    }
}