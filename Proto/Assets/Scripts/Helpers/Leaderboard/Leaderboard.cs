using System.Collections.Generic;
using Newtonsoft.Json;

public class Leaderboard
{
    private List<Score> _scores;
    private FileManager _fileManager;
    public Leaderboard(string file)
    {
        _scores = Load(file);
    }

    private List<Score> Load(string file)
    {
        _fileManager = new FileManager(file);
        var json = _fileManager.Read();

        return json == "" ? new List<Score>() : JsonConvert.DeserializeObject<List<Score>>(json);
    }

    public void AddScore(Score newScore)
    {
        if (_scores.Count == 0)
        {
            _scores.Add(newScore);
            return;
        }
        //This should keep the list ordered
        for (var i = 0; i < _scores.Count; i++)
        {
            var score = _scores[i];
            if (score.Time <= newScore.Time) continue;
            _scores.Insert(i, newScore);
            break;
        }
    }

    public void Save()
    {
        //This should keep the list with at most 10 elements
        if (_scores.Count > 10)
        {
            _scores.RemoveRange(9, _scores.Count - 10);
        }
        _fileManager.Write(JsonConvert.SerializeObject(_scores));
    }
}