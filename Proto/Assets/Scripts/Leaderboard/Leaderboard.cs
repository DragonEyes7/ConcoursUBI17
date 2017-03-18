using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class Leaderboard
{
    private readonly List<Score> _scores;
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

    public void Show(RectTransform list)
    {
        foreach (var score in _scores)
        {
            var leaderboardEntry = (GameObject)Object.Instantiate(Resources.Load("LeaderboardField"), list);
            leaderboardEntry.GetComponent<LeaderboardEntry>().SetInfo(score);
        }
    }

    public void AddScore(Score newScore)
    {
        if (_scores.Count == 0)
        {
            _scores.Add(newScore);
            return;
        }
        var inserted = false;
        //This should keep the list ordered
        for (var i = 0; i < _scores.Count; i++)
        {
            var score = _scores[i];
            if (score.Time < newScore.Time) continue;
            _scores.Insert(i, newScore);
            inserted = true;
            break;
        }
        //If we don't yet have 10 scores and this one is the highest one, insert at the end
        if(!inserted && _scores.Count < 10)_scores.Insert(_scores.Count, newScore);
    }

    public void Save()
    {
        //This should keep the list with at most 10 elements
        if (_scores.Count > 10)
        {
            _scores.RemoveRange(9, _scores.Count - 10);
        }
        _fileManager.Write(JsonConvert.SerializeObject(_scores, Formatting.Indented));
    }
}