using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour
{
    [SerializeField]Transform[] m_SpawnPoint;

    void OnJoinedRoom()
    {
        string obj = "";
        int timerInSeconds = 30;
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
                HUD hud = FindObjectOfType<HUD>();
                if (hud)
                {
                    hud.SetPlayer(myPlayer);
                    timerInSeconds = GM.CurrentTimer();
                }
            }

            SetupHUD(myPlayer, obj, timerInSeconds);
        }
        else
        {
            FindObjectOfType<Detective>().enabled = true;

            GameManager GM = FindObjectOfType<GameManager>();
            if(GM)
            {
                timerInSeconds = GM.CurrentTimer();
            }

            SetupHUD(null, obj, timerInSeconds);
        }

        LoadingCompleted();
    }

    void SetupHUD(GameObject myPlayer, string objectives, int timer)
    {
        HUD hud = FindObjectOfType<HUD>();
        if (hud)
        {
            hud.SetPlayer(myPlayer);
            hud.SetObjectives(objectives);
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