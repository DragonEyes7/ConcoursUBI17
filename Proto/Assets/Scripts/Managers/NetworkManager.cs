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
            myPlayer.GetComponent<Action>().enabled = true;
            myPlayer.GetComponent<PlayerSetup>().enabled = true;
            myPlayer.GetComponent<PlayerSetup>().SetupCamera();

            GameManager GM = FindObjectOfType<GameManager>();
            if (GM)
            {
                SetupHUD(myPlayer, GM.CurrentTimer());
                GM.Setup();
            }            
        }
        else
        {
            FindObjectOfType<CamerasController>().SetIntelligence(true);

            GameManager GM = FindObjectOfType<GameManager>();
            if(GM)
            {
                SetupHUD(null, GM.CurrentTimer());
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

    void LoadingCompleted()
    {
        LoadingManager LM = FindObjectOfType<LoadingManager>();
        if (LM)
        {
            LM.ConnectionCompleted();
        }
    }
}