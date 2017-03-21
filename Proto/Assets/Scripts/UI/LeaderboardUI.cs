using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeaderboardUI : MonoBehaviour
{
    [SerializeField] private RectTransform _scoresList;
    [SerializeField] private Text _sceneTitle;
    [SerializeField] private string _defaultScene = "Tuto";


    private const string _path = "./";
    private string _filePath;
    private Leaderboard _leaderboard;
    private const string _leaderboardTitle = "Leaderboard - ";

    private void Start ()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        LoadScene(_defaultScene);
    }

    public void LoadScene(string scene)
    {
        ResetList();
        _filePath = _path + scene + ".json";
        _leaderboard = new Leaderboard(_filePath);
        _leaderboard.Show(_scoresList);
        UpdateLeaderboardTitle(scene);
    }

    private void ResetList()
    {
        for (var i = 0; i < _scoresList.childCount; ++i)
        {
            Destroy(_scoresList.GetChild(i).gameObject);
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void UpdateLeaderboardTitle(string sceneTitle)
    {
        _sceneTitle.text = _leaderboardTitle + sceneTitle;
    }
}
