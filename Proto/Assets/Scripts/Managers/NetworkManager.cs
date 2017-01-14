using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour
{
    [SerializeField]Transform[] m_SpawnPoint;

    void OnJoinedRoom()
    {
        if(PhotonNetwork.playerList.Length - 1 == 0)
        {
            GameObject myPlayer = PhotonNetwork.Instantiate("Player", m_SpawnPoint[PhotonNetwork.playerList.Length - 1].position, m_SpawnPoint[PhotonNetwork.playerList.Length - 1].rotation, 0);
            myPlayer.name = "My Player";
            myPlayer.GetComponentInChildren<Renderer>().material.color = Color.blue;
            myPlayer.GetComponent<Movement>().enabled = true;
        }
        else
        {
            FindObjectOfType<Detective>().enabled = true;
        }

        LoadingCompleted();
    }

    void OnDisconnectedFromPhoton()
    {
        SceneManager.LoadScene("MainMenu");
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