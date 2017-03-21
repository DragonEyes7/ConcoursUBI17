using System;
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
        var levelShown = FindObjectOfType<LevelToLoad>().GetLevelToLoad();
        pos = GetPosOfLevelShown(levelShown);
        _levelName.text = _levels[pos];
        UpdateArrows();
    }

    private int GetPosOfLevelShown(string levelShown)
    {
        for (var i = 0; i < _levels.Length; ++i)
        {
            var level = _levels[i];
            if (level.IndexOf(levelShown, StringComparison.InvariantCultureIgnoreCase) > -1)
                return i;
        }
        //Defaults at loading the first entry if nothing found
        return 0;
    }

    public void LeftArrowClick()
    {
        _levelName.text = _levels[--pos];
        _leaderboardUI.LoadScene(_levels[pos]);
        UpdateArrows();
    }

    public void RightArrowClick()
    {
        _levelName.text = _levels[++pos];
        _leaderboardUI.LoadScene(_levels[pos]);
        UpdateArrows();
    }

    private void UpdateArrows()
    {
        _leftArrow.gameObject.SetActive(true);
        _rightArrow.gameObject.SetActive(true);
        if (pos == 0) _leftArrow.gameObject.SetActive(false);
        if (_levels.Length <= pos + 1)_rightArrow.gameObject.SetActive(false);
    }
}
