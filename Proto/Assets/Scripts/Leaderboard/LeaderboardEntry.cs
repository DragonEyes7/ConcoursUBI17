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
        _time.text = score.Time.ToString();
        _penality.text = score.Penality.ToString();
    }
}