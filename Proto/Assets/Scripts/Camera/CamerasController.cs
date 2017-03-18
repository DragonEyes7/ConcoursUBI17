using UnityEngine;
using System.Collections.Generic;

public class CamerasController : MonoBehaviour
{
    [SerializeField]Dictionary<string, List<GameObject>> m_CameraGroups = new Dictionary<string, List<GameObject>>();
    [SerializeField]Camera m_SceneCamera;
    PhotonView m_PhotonView;

    public GameObject m_LastCamera;

    GameObject _CurrentCam;

    bool m_IsIntelligence = false;

    AudioSource _AudioSource;

    void Start()
    {
        _AudioSource = GetComponent<AudioSource>();
        m_PhotonView = GetComponent<PhotonView>();
    }

    public void SetIntelligence(bool value)
    {
        m_IsIntelligence = value;
        if (m_IsIntelligence)
        {
            m_SceneCamera.gameObject.SetActive(false);
            string firstCameraGroup = m_LastCamera.GetComponent<HackableCamera>().CameraGroup();
            AddToCameraList(m_LastCamera, firstCameraGroup);
            m_CameraGroups[firstCameraGroup][0].GetComponent<PhotonView>().RequestOwnership();

            SetActiveCamera(m_LastCamera, m_LastCamera);           
        }
    }

    public void SetActiveCamera(GameObject currentCam, GameObject lastCam)
    {
        m_PhotonView.RPC("RPCActiveCamera"
            , PhotonTargets.All
            , m_CameraGroups[currentCam.GetComponent<HackableCamera>().CameraGroup()].IndexOf(currentCam)
            , currentCam.GetComponent<HackableCamera>().CameraGroup()
            , m_CameraGroups[lastCam.GetComponent<HackableCamera>().CameraGroup()].IndexOf(lastCam)
            , lastCam.GetComponent<HackableCamera>().CameraGroup());

        lastCam.GetComponentInChildren<Camera>().enabled = false;
        lastCam.GetComponent<CameraMovement>().enabled = false;
        lastCam.GetComponentInChildren<IntelligenceAction>().enabled = false;
        lastCam.GetComponent<AudioListener>().enabled = false;
        _CurrentCam = currentCam;
        currentCam.GetComponentInChildren<Camera>().enabled = true;
        currentCam.GetComponent<CameraMovement>().enabled = true;
        currentCam.GetComponentInChildren<IntelligenceAction>().enabled = true;
        currentCam.GetComponent<AudioListener>().enabled = true;

        m_LastCamera = currentCam;
    }

    [PunRPC]
    void RPCActiveCamera(int currentCamIndex, string currentCamGroup, int lastCamIndex, string lastCamGroup)
    {
        m_CameraGroups[lastCamGroup][lastCamIndex].GetComponent<Renderer>().material.color = Color.white;

        m_CameraGroups[currentCamGroup][currentCamIndex].GetComponent<Renderer>().material.color = Color.red;
    }

    public void AddToCameraList(GameObject cameraToAdd, string cameraGroup)
    {
        if(m_IsIntelligence)
        {
            if (!m_CameraGroups.ContainsKey(cameraGroup))
            {
                m_CameraGroups.Add(cameraGroup, new List<GameObject>());
            }
            m_CameraGroups[cameraGroup].Add(cameraToAdd);
            m_PhotonView.RPC("RPCAddCamera", PhotonTargets.Others, cameraToAdd.GetPhotonView().instantiationId, cameraGroup);

            cameraToAdd.GetComponent<PhotonView>().RequestOwnership();

            TakeControl(cameraToAdd, cameraGroup);
        }
    }

    [PunRPC]
    void RPCAddCamera(int id, string cameraGroup)
    {
        HackableCamera[] HCs = FindObjectsOfType<HackableCamera>();

        foreach (HackableCamera hc in HCs)
        {
            if(hc.gameObject.GetPhotonView().instantiationId == id)
            {
                if (!m_CameraGroups.ContainsKey(cameraGroup))
                {
                    m_CameraGroups.Add(cameraGroup, new List<GameObject>());
                }
                m_CameraGroups[cameraGroup].Add(hc.gameObject);
                return;
            }            
        }
    }

    public bool ContaintCamera(GameObject camera, string cameraGroup)
    {
        return m_CameraGroups.ContainsKey(cameraGroup) ? false : m_CameraGroups[cameraGroup].Contains(camera);
    }

    public void TakeControl(GameObject camera, string cameraGroup)
    {
        if(!m_CameraGroups.ContainsKey(cameraGroup))
        {
            AddToCameraList(camera, cameraGroup);
        }
        else if(!m_CameraGroups[cameraGroup].Contains(camera))
        {
            AddToCameraList(camera, cameraGroup);
        }
        SetActiveCamera(camera, m_LastCamera);
        _AudioSource.Play();
    }

    public List<string> GetCameraGroupList()
    {
        return new List<string>(m_CameraGroups.Keys);
    }

    public List<GameObject> GetGroupCameras(string group)
    {
        return m_CameraGroups[group];
    }

    public GameObject GetActiveCamera()
    {
        return _CurrentCam;
    }
}