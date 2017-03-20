using UnityEngine;
using UnityEngine.UI;

public class SceneSelector : MonoBehaviour
{
    [SerializeField] private Text _levelName;
    [SerializeField] private string[] _levels;
    [SerializeField] private Button _leftArrow;
    [SerializeField] private Button _rightArrow;
    private int pos = 0;
    private LeaderboardUI _leaderboardUI;

    private void Start()
    {
        _leaderboardUI = FindObjectOfType<LeaderboardUI>();
        _levelName.text = _levels[pos];
        _leftArrow.gameObject.SetActive(false);
        if (_levels.Length <= 1) _rightArrow.gameObject.SetActive(false);
    }

    public void LeftArrowClick()
    {
        Debug.Log("Left Click");
        _levelName.text = _levels[--pos];
        if (pos == 0) _leftArrow.gameObject.SetActive(false);
        if (_levels.Length > 1) _rightArrow.gameObject.SetActive(true);
        _leaderboardUI.LoadScene(_levels[pos]);
    }

    public void RightArrowClick()
    {
        Debug.Log("Right click");
        _levelName.text = _levels[++pos];
        if (_levels.Length <= pos + 1)_rightArrow.gameObject.SetActive(false);
        _leftArrow.gameObject.SetActive(true);
        _leaderboardUI.LoadScene(_levels[pos]);
    }
}
