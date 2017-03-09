using System.Collections;
using UnityEngine;

public class LeaderboardUI : MonoBehaviour
{
    [SerializeField]RectTransform _scoresList;

    void Start ()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void BackToMainMenu()
    {
        PhotonNetwork.LoadLevel("MainMenu");
    }

}