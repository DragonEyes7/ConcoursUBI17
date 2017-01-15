using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]Transform[] m_DoorsSpawn;
    [SerializeField]int m_NumberOfTargets = 1;
    int[][] m_TargetsCharacteristics;

    public void Setup()
    {
        ValideNumberOfTargets();
        FindRandomTargets();
        SetupRandomExit();
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
}