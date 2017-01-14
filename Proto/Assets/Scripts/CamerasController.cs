using UnityEngine;

public class CamerasController : MonoBehaviour
{
    [SerializeField]GameObject[] m_CameraObjects;
    PhotonView m_PhotonView;

    void Start ()
    {
        m_PhotonView = GetComponent<PhotonView>();
    }
	
	void Update ()
    {
		
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