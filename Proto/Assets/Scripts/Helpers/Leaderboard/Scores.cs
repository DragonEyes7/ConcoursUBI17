using System;
using System.Collections.Generic;

[Serializable]
public class Scores
{
    public List<Score> AllScores;

    public Scores(List<Score> scores)
    {
        AllScores = scores;
    }

    public Scores()
    {
        AllScores = new List<Score>();
    }
}