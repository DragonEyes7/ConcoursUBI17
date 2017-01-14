﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour
{
    [SerializeField]Transform[] m_SpawnPoint;

    void OnJoinedRoom()
    {
        GameObject myPlayer = PhotonNetwork.Instantiate("Player", m_SpawnPoint[PhotonNetwork.playerList.Length-1].position, m_SpawnPoint[PhotonNetwork.playerList.Length-1].rotation, 0);
        myPlayer.name = "My Player";
        myPlayer.GetComponentInChildren<Renderer>().material.color = Color.blue;
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