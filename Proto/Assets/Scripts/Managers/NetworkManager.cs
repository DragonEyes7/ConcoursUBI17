using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour
{
    [SerializeField]Transform[] m_SpawnPoint;

    void OnJoinedRoom()
    {
        string obj = "";
        float timerInSeconds = 30f;
        if(PhotonNetwork.playerList.Length - 1 == 0)
        {
            GameObject myPlayer = PhotonNetwork.Instantiate("Player", m_SpawnPoint[PhotonNetwork.playerList.Length - 1].position, m_SpawnPoint[PhotonNetwork.playerList.Length - 1].rotation, 0);
            myPlayer.name = "My Player";
            myPlayer.GetComponentInChildren<Renderer>().material.color = Color.blue;
            myPlayer.GetComponent<Movement>().enabled = true;
            myPlayer.GetComponent<Action>().enabled = true;
            myPlayer.GetComponent<PlayerSetup>().enabled = true;
            myPlayer.GetComponent<PlayerSetup>().SetupCamera();

            TutoManager TM = FindObjectOfType<TutoManager>();
            if (TM)
            {
                HUD hud = FindObjectOfType<HUD>();
                if (hud)
                {
                    hud.SetPlayer(myPlayer);
                    obj = TM.CurrentObjectiveDefuser();
                    timerInSeconds = TM.CurrentTimer();
                }
            }
            else
            {
                FindObjectOfType<GameManager>().Setup();
            }

            SetupHUD(myPlayer, obj, timerInSeconds);
        }
        else
        {
            FindObjectOfType<Detective>().enabled = true;

            TutoManager TM = FindObjectOfType<TutoManager>();
            if(TM)
            {
                obj = TM.CurrentObjectiveDetective();
                timerInSeconds = TM.CurrentTimer();
            }

            SetupHUD(null, obj, timerInSeconds);
        }

        LoadingCompleted();
    }

    void SetupHUD(GameObject myPlayer, string objectives, float timer)
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