using UnityEngine;

public class Leaderboard
{
    private Scores _scores;
    private FileManager _fileManager;
    public Leaderboard(string file)
    {
        _scores = Load(file);
    }

    private Scores Load(string file)
    {
        _fileManager = new FileManager(file);
        var json = _fileManager.Read();

        return json == "" ? new Scores() : JsonUtility.FromJson<Scores>(json);
    }

    public void AddScore(Score newScore)
    {
        Debug.Log("Adding Score : " + newScore.Name + " - " + newScore.Time);
        if (_scores.AllScores.Count == 0)
        {
            Debug.Log("Had no scores, added.");
            _scores.AllScores.Add(newScore);
            return;
        }
        //This should keep the list ordered
        for (var i = 0; i < _scores.AllScores.Count; i++)
        {
            var score = _scores.AllScores[i];
            if (score.Time <= newScore.Time) continue;
            _scores.AllScores.Insert(i, newScore);
            break;
        }
    }

    public void Save()
    {
        Debug.Log("Scores count : " + _scores.AllScores.Count);
        //This should keep the list with at most 10 elements
        if (_scores.AllScores.Count > 10)
        {
            _scores.AllScores.RemoveRange(9, _scores.AllScores.Count - 10);
        }
        _fileManager.Write(JsonUtility.ToJson(_scores));
    }
}