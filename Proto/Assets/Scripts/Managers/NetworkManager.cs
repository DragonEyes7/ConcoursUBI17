using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour
{
    [SerializeField]Transform[] m_SpawnPoint;

    void OnJoinedRoom()
    {
        TimeStopper.StopTime();
        GameManager GM = FindObjectOfType<GameManager>();

        if (PhotonNetwork.playerList.Length - 1 == 0)
        {
            GameObject myPlayer = PhotonNetwork.Instantiate("Player", m_SpawnPoint[PhotonNetwork.playerList.Length - 1].position, m_SpawnPoint[PhotonNetwork.playerList.Length - 1].rotation, 0);
            myPlayer.name = "My Player";
            myPlayer.GetComponent<Movement>().enabled = true;
            myPlayer.GetComponent<Action>().enabled = true;
            myPlayer.GetComponent<PlayerSetup>().enabled = true;
            myPlayer.GetComponent<PlayerSetup>().SetupCamera();
            myPlayer.GetComponent<AudioListener>().enabled = true;

            if (GM)
            {
                SetupHUD(myPlayer, GM.CurrentTimer());
                GM.Setup(true);
            }
        }
        else
        {
            CamerasController CC = FindObjectOfType<CamerasController>();
            CC.SetIntelligence(true);

            if (GM)
            {
                SetupHUD(null, GM.CurrentTimer());
                GM.Setup(false);
            }            
        }

        LoadingCompleted();
    }

    void SetupHUD(GameObject myPlayer, int timer)
    {
        HUD hud = FindObjectOfType<HUD>();
        if (hud)
        {
            hud.SetPlayer(myPlayer);
            hud.SetLevelTimer(timer);
        }
    }

    void OnDisconnectedFromPhoton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        if(!FindObjectOfType<GameManager>().GameCompleted())
        {
            PhotonNetwork.Disconnect();
        }        
    }

    public void LoadingCompleted()
    {
        LoadingManager LM = FindObjectOfType<LoadingManager>();
        if (LM)
        {
            LM.ConnectionCompleted();
        }
    }
}