using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField]int _levelTimer = 30;
    [SerializeField]AudioClip[] _AudioClips;
    [SerializeField]AudioSource _BGM;

    List<Clue> m_IntelligenceClues = new List<Clue>();

    Dictionary<string, string> m_TargetsCharacteristics;
    int m_InnocentTargetsIntercepted;

    List<string> _HatList = new List<string>();
    List<string> _FacialList = new List<string>();
    List<string> _AccessoryList = new List<string>();

    bool _GameCompleted = false;
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

        if (m_IsMaster)
        {
            LevelGenerator LG = FindObjectOfType<LevelGenerator>();
            if (LG)
            {
                LG.Setup();
            }

            FindObjectOfType<NPCManager>().Setup();
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

        ExitGames.Client.Photon.Hashtable prop = new ExitGames.Client.Photon.Hashtable();

        GameObject[] HatList = Resources.LoadAll<GameObject>("Hats");
        foreach (GameObject hat in HatList)
        {
            _HatList.Add(hat.name);
        }

        _HatList.Remove(m_TargetsCharacteristics["Hat"]);

        GameObject[] FacialList = Resources.LoadAll<GameObject>("Facials");
        foreach (GameObject fac in FacialList)
        {
            _FacialList.Add(fac.name);
        }

        _FacialList.Remove(m_TargetsCharacteristics["Facial"]);

        GameObject[] AccList = Resources.LoadAll<GameObject>("Accessories");
        foreach (GameObject acc in AccList)
        {
            _AccessoryList.Add(acc.name);
        }

        _AccessoryList.Remove(m_TargetsCharacteristics["Accessory"]);

        prop.Add("Targets", m_TargetsCharacteristics);
        prop.Add("HatList", _HatList.ToArray());
        prop.Add("FacialList", _FacialList.ToArray());
        prop.Add("AccessoryList", _AccessoryList.ToArray());

        PhotonNetwork.room.SetCustomProperties(prop);
    }

    void OnPhotonCustomRoomPropertiesChanged(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey("Targets"))
        {
            m_TargetsCharacteristics = (Dictionary<string, string>)propertiesThatChanged["Targets"];
        }
        if (propertiesThatChanged.ContainsKey("HatList"))
        {
            string[] list = (string[])propertiesThatChanged["HatList"];
            foreach(string l in list)
            {
                _HatList.Add(l);
            }
        }
        if (propertiesThatChanged.ContainsKey("FacialList"))
        {
            string[] list = (string[])propertiesThatChanged["FacialList"];
            foreach (string l in list)
            {
                _FacialList.Add(l);
            }
        }
        if (propertiesThatChanged.ContainsKey("AccessoryList"))
        {
            string[] list = (string[])propertiesThatChanged["AccessoryList"];
            foreach (string l in list)
            {
                _AccessoryList.Add(l);
            }
        }
        if (propertiesThatChanged.ContainsKey("Objectives"))
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
        if(!m_ObjectivesCompleted) ++m_InnocentTargetsIntercepted;
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
        switch (clueType)
        {
            case Clue.ClueStrengthType.WEAKHAT:
                CreateWeakClue(clueGiverID, clueType, _HatList);
                break;
            case Clue.ClueStrengthType.WEAKFACIAL:
                CreateWeakClue(clueGiverID, clueType, _FacialList);
                break;
            case Clue.ClueStrengthType.WEAKACCESSORIES:
                CreateWeakClue(clueGiverID, clueType, _AccessoryList);
                break;
            case Clue.ClueStrengthType.MEDHAT:
                CreateMediumClue(clueGiverID, clueType, "HatMat");
                break;
            case Clue.ClueStrengthType.MEDFACIAL:
                CreateMediumClue(clueGiverID, clueType, "FacialMat");
                break;
            case Clue.ClueStrengthType.MEDACCESSORIES:
                CreateMediumClue(clueGiverID, clueType, "AccessoryMat");
                break;
            case Clue.ClueStrengthType.STRONGHAT:
                CreateStrongClue(clueGiverID, clueType, Clue.ClueStrengthType.WEAKHAT, "Hat", _HatList);
                break;
            case Clue.ClueStrengthType.STRONGFACIAL:
                CreateStrongClue(clueGiverID, clueType, Clue.ClueStrengthType.WEAKFACIAL, "Facial", _FacialList);
                break;
            case Clue.ClueStrengthType.STRONGACCESSORIES:
                CreateStrongClue(clueGiverID, clueType, Clue.ClueStrengthType.WEAKACCESSORIES, "Accessory", _AccessoryList);
                break;
            default:                
                Debug.Log("Something when wrong in the timeline a value was lost. Reboot your router!");
                break;
        }
    }

    void CreateWeakClue(int clueGiverID, Clue.ClueStrengthType clueType, List<string> itemList)
    {
        if (itemList.Count == 0)
            return;
        string res = itemList[Random.Range(0, itemList.Count)];
        itemList.Remove(res);

        Clue clue = new Clue();
        clue.ClueGiverID = clueGiverID;
        clue.ClueStrength = clueType;
        clue.ClueString = "The target doesn't wears a " + res + ".";

        foreach (Clue sclue in m_IntelligenceClues)
        {
            if (sclue.ClueString == clue.ClueString)
            {
                return;
            }
        }

        m_IntelligenceClues.Add(clue);
    }

    void CreateMediumClue(int clueGiverID, Clue.ClueStrengthType clueType, string item)
    {
        Clue clue = new Clue();
        bool found = false;
        foreach (Clue sclue in m_IntelligenceClues)
        {
            if (sclue.ClueStrength == Clue.ClueStrengthType.MEDHAT
              || sclue.ClueStrength == Clue.ClueStrengthType.MEDFACIAL
              || sclue.ClueStrength == Clue.ClueStrengthType.MEDACCESSORIES)
            {
                if (sclue.ClueColors.ContainsKey(m_TargetsCharacteristics[item]))
                {
                    sclue.ClueColors[m_TargetsCharacteristics[item]] = ++sclue.ClueColors[m_TargetsCharacteristics[item]];
                }
                else
                {
                    sclue.ClueColors.Add(m_TargetsCharacteristics[item], 1);
                }
                clue = sclue;
                found = true;
            }
        }

        if (!found)
        {
            clue.ClueGiverID = clueGiverID;
            clue.ClueStrength = clueType;
            clue.ClueColors.Add(m_TargetsCharacteristics[item], 1);
            clue.ClueString = "The target wears something "+ m_TargetsCharacteristics[item] + ".";
            m_IntelligenceClues.Add(clue);
        }
        else
        {
            string result = "The target wears ";
            List<string> parts = new List<string>();
            foreach(KeyValuePair<string, int> str in clue.ClueColors)
            {
                switch(str.Value)
                {
                    case 0:
                        continue;
                    case 1:
                        parts.Add("one");
                        break;
                    case 2:
                        parts.Add("two");
                        break;
                    case 3:
                        parts.Add("three");
                        break;
                    default:
                        parts.Add("How is it even possible... you might be the luckiest man alive!");
                        break;
                }
                parts.Add(str.Key);
            }

            if(parts.Count == 2)
            {
                result += parts[0] + " " + parts[1] + " items.";
            }
            else if(parts.Count == 4)
            {
                if(parts[0] == "one" && parts[2] == "one")
                {
                    result += " something " + parts[1] + " and something " + parts[3];
                }
                else if(parts[0] == "two")
                {
                    result += parts[0] + " " + parts[1] + " items and " + parts[2] + " " + parts[3] + ".";
                }
                else if(parts[2] == "two")
                {
                    result += parts[2] + " " + parts[3] + " items and " + parts[0] + " " + parts[1] + ".";
                }
            }
            else if(parts.Count == 6)
            {
                result += "something " + parts[1] + ", something " + parts[3] + " and something " + parts[5] + ".";
            }

            clue.ClueString = result;

            //two blue = two blue (2) | three blue = three blue (2)
            //blue and red = one blue one red (4) | two blue and one red = two blue one red (4)
            //blue, red, white = one blue one red one white (6)
        }
    }

    void CreateStrongClue(int clueGiverID, Clue.ClueStrengthType clueType, Clue.ClueStrengthType type, string item, List<string> itemList)
    {
        List<Clue> toRemove = new List<Clue>();
        foreach (Clue sclue in m_IntelligenceClues)
        {
            if (sclue.ClueStrength == type)
            {
                toRemove.Add(sclue);                
            }
        }
        foreach(Clue sclue in toRemove)
        {
            m_IntelligenceClues.Remove(sclue);
        }
        itemList.Clear();

        Clue clue = new Clue();
        clue.ClueGiverID = clueGiverID;
        clue.ClueStrength = clueType;
        clue.ClueString = "The target wears " + m_TargetsCharacteristics[item];

        m_IntelligenceClues.Add(clue);
    }

    public void AddCluesToIntelligence(int clueGiverID, Clue.ClueStrengthType clueType)
    {
        foreach(Clue clue in m_IntelligenceClues)
        {
            if((int)clueType >= 3 && clue.ClueGiverID == clueGiverID && clue.ClueStrength == clueType)
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
        _BGM.clip = _AudioClips[1];
        _BGM.pitch = 1f;
        _BGM.Play();

        float duration = 3f;
        FindObjectOfType<HUD>().ShowVictoryMessage("Game Over");
        Invoke("Disconnect", duration);
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("Leaderboard");
    }

    public void SetGameCompleted(bool value)
    {
        _GameCompleted = value;
    }

    public bool GameCompleted()
    {
        return _GameCompleted;
    }

    public void PlayVictoryMusic()
    {
        _BGM.clip = _AudioClips[0];
        _BGM.pitch = 1f;
        _BGM.Play();
    }
}