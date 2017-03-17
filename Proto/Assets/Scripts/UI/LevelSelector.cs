using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private Text _levelName;
    [SerializeField] private string[] _levels;
    [SerializeField] private Button _leftArrow;
    [SerializeField] private Button _rightArrow;
    private int pos = 0;

    private void Start()
    {
        _levelName.text = _levels[pos];
        _leftArrow.gameObject.SetActive(false);
        if (_levels.Length <= 1) _rightArrow.gameObject.SetActive(false);
    }

    public void LeftArrowClick()
    {
        _levelName.text = _levels[--pos];
        if (pos == 0) _leftArrow.gameObject.SetActive(false);
        if (_levels.Length > 1) _rightArrow.gameObject.SetActive(true);
    }

    public void RightArrowClick()
    {
        _levelName.text = _levels[++pos];
        if (_levels.Length <= pos + 1) _rightArrow.gameObject.SetActive(false);
        _leftArrow.gameObject.SetActive(true);
    }
}