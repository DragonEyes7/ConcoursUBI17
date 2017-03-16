using UnityEngine;
using UnityEngine.UI;

public class LeaderboardEntry : MonoBehaviour
{

    [SerializeField] private Text _name;
    [SerializeField] private Text _time;
    [SerializeField] private Text _penality;

    public void SetInfo(Score score)
    {
        _name.text = score.Name;
        var time = score.Time;
        _time.text = string.Format("{0}:{1:00}", time / 60, time % 60);
        _penality.text = score.Penality.ToString();
    }
}