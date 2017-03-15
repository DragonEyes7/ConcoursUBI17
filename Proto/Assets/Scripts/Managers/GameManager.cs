using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField]Transform[] m_DoorsSpawn;
    [SerializeField]int _levelTimer = 30;

    List<Clue> m_IntelligenceClues = new List<Clue>();

    Dictionary<string, string> m_TargetsCharacteristics;
    int m_InnocentTargetsIntercepted;

    AudioSource _AudioSource;

    List<string> _HatList = new List<string>();

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
        _AudioSource = GetComponent<AudioSource>();

        if (m_IsMaster)
        {
            LevelGenerator LG = FindObjectOfType<LevelGenerator>();
            if (LG)
            {
                LG.Setup();
            }

            FindObjectOfType<NPCManager>().Setup();
            FindRandomTargets();

            GameObject[] HatList = Resources.LoadAll<GameObject>("Hat");
            foreach (GameObject hat in HatList)
            {
                _HatList.Add(hat.name);
            }

            _HatList.Remove(m_TargetsCharacteristics["Hat"]);
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
                ExitGames.Client.Photon.Hashtable prop = PhotonNetwork.room.CustomProperties;
                prop.Add("PlayerReady", m_Ready);
                PhotonNetwork.room.SetCustomProperties(prop);
            }
        }
    }

    void FindRandomTargets()
    {
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
        }
        if(propertiesThatChanged.ContainsKey("Objectives"))
        {
            m_ObjectivesCompleted = (bool)propertiesThatChanged["Objectives"];
        }
        if(propertiesThatChanged.ContainsKey("PlayerReady"))
        {
            m_Ready = (bool)propertiesThatChanged["PlayerReady"];

            if(m_Ready)
            {
                FindObjectOfType<SyncPlayersUI>().PlayerReady();
            }
        }        
    }

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

    public void ConstructAndAddClue(int clueGiverID, Clue.ClueStrengthType clueType)
    {
        string result = "";
        Clue clue = new Clue();
        switch(clueType)
        {
            case Clue.ClueStrengthType.WEAKHAT:
                if (_HatList.Count == 0)
                    return;
                string res = _HatList[Random.Range(0, _HatList.Count)];
                _HatList.Remove(res);
                result = ("The target doesn't wears a " + res);
                break;
            case Clue.ClueStrengthType.WEAKFACIAL:
                break;
            case Clue.ClueStrengthType.WEAKACCESSORIES:
                break;
            case Clue.ClueStrengthType.MEDHAT:
                break;
            case Clue.ClueStrengthType.MEDFACIAL:
                break;
            case Clue.ClueStrengthType.MEDACCESSORIES:
                break;
            case Clue.ClueStrengthType.STRONGHAT:
                foreach (Clue sclue in m_IntelligenceClues)
                {
                    if (sclue.ClueStrength == Clue.ClueStrengthType.WEAKHAT)
                    {
                        m_IntelligenceClues.Remove(sclue);
                    }
                }
                _HatList.Clear();
                result = "The target wears a " + m_TargetsCharacteristics["Hat"];
                break;
            case Clue.ClueStrengthType.STRONGFACIAL:
                break;
            case Clue.ClueStrengthType.STRONGACCESSORIES:
                break;
            default:                
                result = "Something when wrong in the timeline a value was lost. Reboot your router!";
                break;
        }

        clue.ClueGiverID = clueGiverID;
        clue.ClueStrength = clueType;
        clue.ClueString = result;

        m_IntelligenceClues.Add(clue);
    }

    public void AddCluesToIntelligence(int clueGiverID, Clue.ClueStrengthType clueType)
    {
        foreach(Clue clue in m_IntelligenceClues)
        {
            if(clue.ClueGiverID == clueGiverID)
            {
                return;
            }
        }

        ConstructAndAddClue(clueGiverID, clueType);
    }

    public List<Clue> GetIntelligenceClues()
    {
        return m_IntelligenceClues;
    }

    public void Defeat()
    {
        _AudioSource.Play();
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