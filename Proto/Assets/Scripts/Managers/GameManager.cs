using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField]Transform[] m_DoorsSpawn;
    [SerializeField]int m_NumberOfTargets = 1;
    [SerializeField]int _levelTimer = 30;
    //[SerializeField]Material[] m_Hairs;
    //[SerializeField]Material[] m_Noses;
    //[SerializeField]Material[] m_Backpacks;

    Dictionary<string, string> m_AgentClues = new Dictionary<string, string>();
    Dictionary<string, string> m_IntelligenceClues = new Dictionary<string, string>();

    Dictionary<string, string> m_TargetsCharacteristics;
    int m_InnocentTargetsIntercepted;

    bool m_ObjectivesCompleted = false;
    bool m_Ready = false;
    bool m_IsMaster;

    public int CurrentTimer()
    {
        return _levelTimer;
    }

    public bool isMaster
    {
        get { return m_IsMaster; }
        set { m_IsMaster = value; }
    }

    public void Setup(bool isMaster)
    {
        m_IsMaster = isMaster;
        if(m_IsMaster)
        {
            FindObjectOfType<NPCManager>().Setup();
            ValideNumberOfTargets();
            FindRandomTargets();
        }
        else
        {
            m_Ready = true;

            if (PhotonNetwork.room.CustomProperties.ContainsKey("PlayerReady"))
            {
                ExitGames.Client.Photon.Hashtable prop = PhotonNetwork.room.CustomProperties;
                prop["PlayerReady"] = m_Ready;
                PhotonNetwork.room.SetCustomProperties(prop);
            }
            else
            {
                ExitGames.Client.Photon.Hashtable prop = new ExitGames.Client.Photon.Hashtable();
                prop.Add("PlayerReady", m_Ready);
                PhotonNetwork.room.SetCustomProperties(prop);
            }
        }
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
            int r = Random.Range(0, i);
            Characteristics tmp = targets[i];
            targets[i] = targets[r];
            targets[r] = tmp;
        }

        m_TargetsCharacteristics = new Dictionary<string, string>();

        GameObject NPCManager = GameObject.FindGameObjectWithTag("NPCManager");
        m_TargetsCharacteristics = NPCManager.GetComponent<NPCManager>().GetTargetCharacteristics();

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
            m_TargetsCharacteristics = (Dictionary<string, string>)propertiesThatChanged["Targets"];
            //DebugShowTarget();
        }
        else if(propertiesThatChanged.ContainsKey("Objectives"))
        {
            m_ObjectivesCompleted = (bool)propertiesThatChanged["Objectives"];
        }
        else if(propertiesThatChanged.ContainsKey("PlayerReady"))
        {
            m_Ready = (bool)propertiesThatChanged["PlayerReady"];

            if(m_Ready)
            {
                FindObjectOfType<SyncPlayersUI>().PlayerReady();
            }
        }        
    }

    /*
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
    */

    public Dictionary<string, string> GetTargets()
    {
        return m_TargetsCharacteristics;
    }

    public void ValidateTarget(int NPCID)
    {
        m_ObjectivesCompleted = NPCID == 0;
        m_InnocentTargetsIntercepted = m_ObjectivesCompleted ? m_InnocentTargetsIntercepted : ++m_InnocentTargetsIntercepted;
    }

    public bool ObjectivesCompleted()
    {
        return m_ObjectivesCompleted;
    }

    public int GetInnocentTargetIntercepted()
    {
        return m_InnocentTargetsIntercepted;
    }

    /*
    public Material GetPartMaterial(int part, int index)
    {
        switch(part)
        {
            case 0:
                return m_Hairs[index];
            case 1:
                return m_Noses[index];
            case 2:
                return m_Backpacks[index];
            default:
                Debug.LogError("Wront part!");
                break;
        }

        return null;
    }
    */

    public string GetTargetClue(string part)
    {
        if (m_TargetsCharacteristics.ContainsKey(part))
        {
            return m_TargetsCharacteristics[part];
        }
        else
        {
            return "";
        }
    }

    public void AddCluesToIntelligence(string part, string clue)
    {
        if(!m_IntelligenceClues.ContainsKey(part))
        {
            m_IntelligenceClues.Add(part, clue);
        }       
    }

    public bool AgentHasClues()
    {
        return m_AgentClues.Count > 0;
    }

    public Dictionary<string, string> GetIntelligenceClues()
    {
        return m_IntelligenceClues;
    }

    public void Defeat()
    {
        float duration = 3f;
        FindObjectOfType<HUD>().ShowMessages("Game Over", duration);
        Invoke("Disconnect", duration);
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel("Leaderboard");
    }
}