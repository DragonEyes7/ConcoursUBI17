using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ServerUI : MonoBehaviour
{
    [SerializeField]InputField m_LevelToLoad;
    [SerializeField]InputField m_ServerNameField;
    [SerializeField]RectTransform m_ServerList;
    List<GameObject> m_ServerButtons = new List<GameObject>();

	void Start ()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PhotonNetwork.ConnectUsingSettings("Proto v0.1");
        StartCoroutine(WaitForConnection());        
    }

    public void CreateServer()
    {
        m_ServerNameField.text = m_ServerNameField.text != "" ? m_ServerNameField.text : "Server " + (PhotonNetwork.GetRoomList().Length + 1);
        PhotonNetwork.CreateRoom(m_ServerNameField.text, null, null);
        PhotonNetwork.LoadLevel(m_LevelToLoad.text);
    }

    public void LoadLeaderboard()
    {
        PhotonNetwork.LoadLevel("Leaderboard");
    }

    public void GetServerList()
    {
        foreach(GameObject serverbutton in m_ServerButtons)
        {
            Destroy(serverbutton);
        }

        m_ServerButtons.Clear();

        foreach (RoomInfo roomInfo in PhotonNetwork.GetRoomList())
        {
            GameObject serverbutton = (GameObject)Instantiate(Resources.Load("ServerJoinButton"), m_ServerList);
            serverbutton.GetComponentInChildren<Text>().text = roomInfo.Name + " : " + roomInfo.PlayerCount + "/2"; //+ roomInfo.MaxPlayers;

            if (roomInfo.PlayerCount >= 2)
            {
                serverbutton.GetComponent<Button>().interactable = false;
            }

            serverbutton.GetComponent<Button>().onClick.AddListener(() => JoinServer(roomInfo.Name));

            m_ServerButtons.Add(serverbutton);
        }
    }

    void JoinServer(string serverName)
    {
        PhotonNetwork.JoinRoom(serverName);
        PhotonNetwork.LoadLevel(m_LevelToLoad.text);
    }

    IEnumerator WaitForConnection()
    {
        yield return new WaitUntil(() => PhotonNetwork.connectionStateDetailed == ClientState.JoinedLobby);
        LoadingCompleted();
    }

    void LoadingCompleted()
    {
        GetServerList();
        LoadingManager LM = FindObjectOfType<LoadingManager>();
        if (LM)
        {
            LM.ConnectionCompleted();
        }
    }
}