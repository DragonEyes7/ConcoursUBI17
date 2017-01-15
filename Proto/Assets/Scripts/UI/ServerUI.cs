using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ServerUI : MonoBehaviour
{
    [SerializeField]InputField m_ServerNameField;
    [SerializeField]RectTransform m_ServerList;

	void Start ()
    {
        PhotonNetwork.ConnectUsingSettings("Proto v0.1");
        StartCoroutine(WaitForConnection());
    }

    public void CreateServer()
    {
        PhotonNetwork.CreateRoom(m_ServerNameField.text, null, null);
        //SceneManager.LoadScene("Level1");
        PhotonNetwork.LoadLevel("Level1");        
    }

    public void GetServerList()
    {
        foreach (RoomInfo roomInfo in PhotonNetwork.GetRoomList())
        {
            GameObject serverbutton = (GameObject)Instantiate(Resources.Load("ServerJoinButton"), m_ServerList);
            serverbutton.GetComponentInChildren<Text>().text = roomInfo.Name + " " + roomInfo.PlayerCount + "/2"; //+ roomInfo.MaxPlayers;

            serverbutton.GetComponent<Button>().onClick.AddListener(() => JoinServer(roomInfo.Name));
        }
    }

    void JoinServer(string serverName)
    {
        PhotonNetwork.JoinRoom(serverName);
        //SceneManager.LoadScene("Level1");
        PhotonNetwork.LoadLevel("Level1");
    }

    IEnumerator WaitForConnection()
    {
        yield return new WaitUntil(() => PhotonNetwork.connectionStateDetailed == ClientState.JoinedLobby);
        LoadingCompleted();
    }

    void LoadingCompleted()
    {
        LoadingManager LM = FindObjectOfType<LoadingManager>();
        if (LM)
        {
            LM.ConnectionCompleted();
        }
    }
}