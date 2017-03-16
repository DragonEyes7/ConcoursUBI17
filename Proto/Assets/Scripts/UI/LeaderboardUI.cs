using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaderboardUI : MonoBehaviour
{
    [SerializeField] private RectTransform _scoresList;
    [SerializeField] private string _filePath;

    private Leaderboard _leaderboard;

    void Start ()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _leaderboard = new Leaderboard(_filePath);
        _leaderboard.Show(_scoresList);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}