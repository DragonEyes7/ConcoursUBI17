using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]Transform[] m_DoorsSpawn;
    [SerializeField]int m_NumberOfTargets = 1;
    int[][] m_TargetsCharacteristics;
    bool[] m_ObjectivesCompleted;
    int m_InnocentTargetsKilled;

    bool m_IsMaster;

    [SerializeField]int _levelTimer = 30;

    public int CurrentTimer()
    {
        return _levelTimer;
    }

    public bool isMaster
    {
        get { return m_IsMaster; }
        set { m_IsMaster = value; }
    }

    public void Setup()
    {
        m_IsMaster = true;
        SetupObjectives();
        ValideNumberOfTargets();
        FindRandomTargets();
        SetupRandomExit();
    }

    void SetupObjectives()
    {
        UpdateObjectivesCompleted(new bool[m_NumberOfTargets]);
    }

    void ValideNumberOfTargets()
    {
        Characteristics[] targets = FindObjectsOfType<Characteristics>();
        if (m_NumberOfTargets > targets.Length)
        {
            m_NumberOfTargets = targets.Length;
        }
    }

    void FindRandomTargets()
    {
        Characteristics[] targets = FindObjectsOfType<Characteristics>();

        for (int i = targets.Length - 1; i > 0; --i)
        {
            int r = Random.Range(0, i = 1);
            Characteristics tmp = targets[i];
            targets[i] = targets[r];
            targets[r] = tmp;
        }

        m_TargetsCharacteristics = new int[m_NumberOfTargets][];

        if (targets.Length > 0)
        {
            for(int i = 0; i < m_NumberOfTargets; ++i)
            {
                m_TargetsCharacteristics[i] = targets[i].GetCharacteristics();
            }
        }

        if (PhotonNetwork.room.CustomProperties.ContainsKey("Targets"))
        {
            ExitGames.Client.Photon.Hashtable prop = PhotonNetwork.room.CustomProperties;
            prop["Targets"] = m_TargetsCharacteristics;
            PhotonNetwork.room.SetCustomProperties(prop);
        }
        else
        {
            ExitGames.Client.Photon.Hashtable prop = new ExitGames.Client.Photon.Hashtable();
            prop.Add("Targets", m_TargetsCharacteristics);
            PhotonNetwork.room.SetCustomProperties(prop);
        }
    }

    void OnPhotonCustomRoomPropertiesChanged(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey("Targets"))
        {
            m_TargetsCharacteristics = (int[][])propertiesThatChanged["Targets"];
            //DebugShowTarget();
        }
        else if(propertiesThatChanged.ContainsKey("Objectives"))
        {
            m_ObjectivesCompleted = (bool[])propertiesThatChanged["Objectives"];
        }
    }

    void SetupRandomExit()
    {
        int random = Random.Range(0, m_DoorsSpawn.Length);
        PhotonNetwork.Instantiate("Door", m_DoorsSpawn[random].position, m_DoorsSpawn[random].rotation, 0);
    }

    void DebugShowTarget()
    {
        Characteristics[] targets = FindObjectsOfType<Characteristics>();

        for(int i = 0; i < targets.Length; ++i)
        {
            int[] characteristics = targets[i].GetCharacteristics();
            bool found = true;
            for (int j = 0; j < characteristics.Length; ++j)
            {                
                if (characteristics[j] != m_TargetsCharacteristics[0][j])
                {
                    found = false;
                    break;
                }
            }

            if(found)
            {
                targets[i].TargetIdentified();
            }
        }
    }

    public int[][] GetTargets()
    {
        return m_TargetsCharacteristics;
    }

    public void ValidateTarget(int[] characteristics)
    {
        bool found = true;
        int i = 0;
        for (int j = 0; j < characteristics.Length; ++j)
        {
            if (characteristics[j] != m_TargetsCharacteristics[i][j])
            {
                found = false;
                ++m_InnocentTargetsKilled;
                break;
            }
        }

        if (found)
        {
            m_ObjectivesCompleted[i] = true;
            UpdateObjectivesCompleted();
        }
    }

    public bool ObjectivesCompleted()
    {
        for(int i = 0; i < m_ObjectivesCompleted.Length; ++i)
        {
            if(!m_ObjectivesCompleted[i])
            {
                return false;
            }
        }

        return true;
    }

    public int GetInnocentTargetKilled()
    {
        return m_InnocentTargetsKilled;
    }

    void UpdateObjectivesCompleted(bool[] array)
    {
        m_ObjectivesCompleted = array;

        if (PhotonNetwork.room.CustomProperties.ContainsKey("Objectives"))
        {
            ExitGames.Client.Photon.Hashtable prop = PhotonNetwork.room.CustomProperties;
            prop["Objectives"] = m_ObjectivesCompleted;
            PhotonNetwork.room.SetCustomProperties(prop);
        }
        else
        {
            ExitGames.Client.Photon.Hashtable prop = new ExitGames.Client.Photon.Hashtable();
            prop.Add("Objectives", m_ObjectivesCompleted);
            PhotonNetwork.room.SetCustomProperties(prop);
        }
    }

    void UpdateObjectivesCompleted()
    {
        if (PhotonNetwork.room.CustomProperties.ContainsKey("Objectives"))
        {
            ExitGames.Client.Photon.Hashtable prop = PhotonNetwork.room.CustomProperties;
            prop["Objectives"] = m_ObjectivesCompleted;
            PhotonNetwork.room.SetCustomProperties(prop);
        }
        else
        {
            ExitGames.Client.Photon.Hashtable prop = new ExitGames.Client.Photon.Hashtable();
            prop.Add("Objectives", m_ObjectivesCompleted);
            PhotonNetwork.room.SetCustomProperties(prop);
        }
    }
}