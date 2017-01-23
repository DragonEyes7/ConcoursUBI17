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
            SetActiveCamera(0);
            m_CameraObjects[0].GetComponent<PhotonView>().RequestOwnership();
            m_CameraObjects[0].GetComponentInChildren<Camera>().enabled = true;
            m_CameraObjects[0].GetComponent<CameraMovement>().enabled = true;
        }
    }

    public void SetActiveCamera(int noCamera)
    {
        m_PhotonView.RPC("RPCActiveCamera", PhotonTargets.All, noCamera);
    }

    [PunRPC]
    void RPCActiveCamera(int noCamera)
    {
        for(int i = 0; i < m_CameraObjects.Length; ++i)
        {
            m_CameraObjects[i].GetComponent<Renderer>().material.color = Color.white;
        }

        m_CameraObjects[noCamera].GetComponent<Renderer>().material.color = Color.red;
    }
}