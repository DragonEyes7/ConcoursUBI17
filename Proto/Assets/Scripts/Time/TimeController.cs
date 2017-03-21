using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeController : MonoBehaviour
{
    public MultipleDelegate Tick = new MultipleDelegate();
    public MultipleDelegate End = new MultipleDelegate();
    private string _filePath = "./";
    [SerializeField] private int _penalizePlayerOnWrongTarget = 20;

    private int _maxTime = 30, _totalTime, _penalties = 0;
    private float _timer;
    public bool _isPlaying;

    public int Time { get; set; }

    public int MaxTime
    {
        get { return _maxTime; }
    }

    public bool IsPlaying
    {
        set { _isPlaying = value; }
    }

    private void FixedUpdate()
    {
        _timer += UnityEngine.Time.deltaTime;
        if (_timer >= 1f && _isPlaying) DoTick();
    }

    private void DoTick()
    {
        _timer = 0;
        Time++;
        _totalTime++;
        Tick.Execute(Time);
        if (Time == _maxTime)
        {
            //Game has ended stop countdown and show the players they f*cked up
            FindObjectOfType<GameManager>().Defeat();
            End.Execute(Time);
            End.Empty();
        }
    }

    public void SaveTime()
    {
        _filePath += SceneManager.GetActiveScene().name + ".json";
        var leaderboard = new Leaderboard(_filePath);
        leaderboard.AddScore(new Score(PhotonNetwork.room.Name, _totalTime, _penalties));
        leaderboard.Save();
    }

    public void WrongTargetIntercepted()
    {
        ++_penalties;
        _totalTime += _penalizePlayerOnWrongTarget;
    }

    public void SetMaxTime(int maxTime)
    {
        _maxTime = maxTime;
    }
}